using Dodatkowe.Data;
using Dodatkowe.DTOs;
using Dodatkowe.Models;
using Microsoft.EntityFrameworkCore;

namespace Dodatkowe.Services;

public interface IDbService
{
    //HELPER
    Task<UpcomingEventDto> GetEventByIdAsync(int id);
    
    //pkt. 1: Utworzenie nowego wydarzenia
    Task<int> CreateEventAsync(CreateEventDto dto);
    
    //pkt. 2: Przypisanie prelegenta do wydarzenia
    Task AssignSpeakerToEventAsync(AssignSpeakerDto dto);
    
    //pkt. 3: Rejestracja uczestnika na wydarzenie
    Task RegisterParticipantAsync(RegisterParticipantDto dto);
    
    //pkt. 4: Anulowanie rejestracji uczestnika
    Task CancelRegistrationAsync(CancelRegistrationDto dto);
    
    //pkt. 5: Pobranie listy wydarzeń z informacją o liczbie wolnych miejsc
    Task<List<UpcomingEventDto>> GetUpcomingEventsAsync();
    
    //pkt. 6: Wygenerowanie raportu udziału uczestników
    Task<List<ParticipantEventReportDto>> GetParticipantReportAsync(int participantId);
    
}

public class DbService(AppDbContext context) : IDbService
{
    private readonly AppDbContext _context = context;
    
    //HELPER
    public async Task<UpcomingEventDto> GetEventByIdAsync(int id)
    {
        var ev = await _context.Events
            .Include(e => e.Registrations)
            .Include(e => e.EventSpeakers)
            .ThenInclude(es => es.Speaker)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (ev == null)
            throw new ArgumentException("Event not found.");

        return new UpcomingEventDto
        {
            Id = ev.Id,
            Title = ev.Title,
            Date = ev.Date,
            MaxParticipants = ev.MaxParticipants,
            RegisteredParticipants = ev.Registrations.Count(r => !r.Cancelled),
            Speakers = ev.EventSpeakers.Select(es => new SpeakerDto
            {
                Id = es.Speaker.Id,
                FullName = es.Speaker.FullName
            }).ToList()
        };
    }

    //pkt. 1 z IDbService 
    public async Task<int> CreateEventAsync(CreateEventDto dto)
    {
        //Data wydarzenia nie może być przeszła
        if (dto.Date < DateTime.UtcNow)
            throw new ArgumentException("Event date must be in the future.");

        //Wprowadzenie danych: tytuł, opis, data, maksymalna liczba uczestników
        var ev = new Event
        {
            Title = dto.Title,
            Description = dto.Description,
            Date = dto.Date,
            MaxParticipants = dto.MaxParticipants
        };

        _context.Events.Add(ev);
        await _context.SaveChangesAsync();
        return ev.Id;
    }
    
    //pkt. 2 z IDbService 
    public async Task AssignSpeakerToEventAsync(AssignSpeakerDto dto) 
    {
        var ev = await _context.Events.FindAsync(dto.EventId);
        if (ev == null)
            throw new ArgumentException("Event not found.");

        var speaker = await _context.Speakers.FindAsync(dto.SpeakerId);
        if (speaker == null)
            throw new ArgumentException("Speaker not found.");

        //Prelegent nie może być przypisany do dwóch wydarzeń w tym samym czasie
        bool conflict = await _context.EventSpeakers
            .AnyAsync(es => es.SpeakerId == dto.SpeakerId &&
                            es.Event.Date == ev.Date);

        if (conflict)
            throw new InvalidOperationException("Speaker is already assigned to another event at the same time.");

        var eventSpeaker = new EventSpeaker
        {
            EventId = dto.EventId,
            SpeakerId = dto.SpeakerId
        };

        _context.EventSpeakers.Add(eventSpeaker);
        await _context.SaveChangesAsync();
    }
    
    //pkt. 3 z IDbService
    public async Task RegisterParticipantAsync(RegisterParticipantDto dto)
    {
        var ev = await _context.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.Id == dto.EventId);

        if (ev == null)
            throw new ArgumentException("Event not found.");

        var participant = await _context.Participants.FindAsync(dto.ParticipantId);
        if (participant == null)
            throw new ArgumentException("Participant not found.");

        //Uczestnik może być zarejestrowany tylko raz na dane wydarzenie
        bool alreadyRegistered = await _context.Registrations
            .AnyAsync(r => r.EventId == dto.EventId && r.ParticipantId == dto.ParticipantId && !r.Cancelled);

        if (alreadyRegistered)
            throw new InvalidOperationException("Participant is already registered for this event.");

        //Sprawdzenie limitu miejsc – jeśli limit osiągnięty, rejestracja niemożliwa
        int registeredCount = ev.Registrations.Count(r => !r.Cancelled);
        if (registeredCount >= ev.MaxParticipants)
            throw new InvalidOperationException("Event is full.");

        var registration = new Registration
        {
            EventId = dto.EventId,
            ParticipantId = dto.ParticipantId,
            RegistrationDate = DateTime.UtcNow,
            Cancelled = false
        };

        _context.Registrations.Add(registration);
        await _context.SaveChangesAsync();
    }
    
    //pkt. 4 z IDbService
    public async Task CancelRegistrationAsync(CancelRegistrationDto dto)
    {
        var registration = await _context.Registrations
            .Include(r => r.Event)
            .FirstOrDefaultAsync(r =>
                r.EventId == dto.EventId &&
                r.ParticipantId == dto.ParticipantId &&
                !r.Cancelled);

        if (registration == null)
            throw new ArgumentException("Active registration not found.");

        //Uczestnik może anulować swój udział do 24 godzin przed rozpoczęciem wydarzenia
        if (registration.Event.Date <= DateTime.UtcNow.AddHours(24))
            throw new InvalidOperationException("Cannot cancel less than 24 hours before the event.");

        registration.Cancelled = true;
        await _context.SaveChangesAsync();
    }
    
    //pkt. 5 z IDbService
    public async Task<List<UpcomingEventDto>> GetUpcomingEventsAsync()
    {
        var now = DateTime.UtcNow;

        var events = await _context.Events
            .Where(e => e.Date >= now)
            .Include(e => e.Registrations)
            .Include(e => e.EventSpeakers)
            .ThenInclude(es => es.Speaker)
            .ToListAsync();

        //Endpoint powinien zwracać wszystkie nadchodzące wydarzenia wraz z
        //nazwami prelegentów
        //liczbą zarejestrowanych uczestników
        //liczbą wolnych miejsc
        return events.Select(e => new UpcomingEventDto
        {
            Id = e.Id,
            Title = e.Title,
            Date = e.Date,
            MaxParticipants = e.MaxParticipants,
            RegisteredParticipants = e.Registrations.Count(r => !r.Cancelled),
            Speakers = e.EventSpeakers.Select(es => new SpeakerDto
            {
                Id = es.Speaker.Id,
                FullName = es.Speaker.FullName
            }).ToList()
        }).ToList();
    }
    
    //pkt. 6 z IDbService
    public async Task<List<ParticipantEventReportDto>> GetParticipantReportAsync(int participantId)
    {
        var participantExists = await _context.Participants.AnyAsync(p => p.Id == participantId);
        if (!participantExists)
            throw new ArgumentException("Participant not found.");

        //Dla danego uczestnika zwróć wszystkie wydarzenia, w których brał udział, z datami i nazwiskami prelegentów.
        var events = await _context.Registrations
            .Where(r => r.ParticipantId == participantId && !r.Cancelled)
            .Include(r => r.Event)
            .ThenInclude(e => e.EventSpeakers)
            .ThenInclude(es => es.Speaker)
            .Select(r => new ParticipantEventReportDto
            {
                EventTitle = r.Event.Title,
                Date = r.Event.Date,
                Speakers = r.Event.EventSpeakers.Select(es => new SpeakerDto
                {
                    Id = es.Speaker.Id,
                    FullName = es.Speaker.FullName
                }).ToList()
            })
            .ToListAsync();

        return events;
    }
}

