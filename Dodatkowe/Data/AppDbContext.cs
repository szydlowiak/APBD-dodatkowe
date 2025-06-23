using Dodatkowe.Models;
using Microsoft.EntityFrameworkCore;

namespace Dodatkowe.Data;

public class AppDbContext : DbContext
{
    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<EventSpeaker> EventSpeakers { get; set; } = null!;
    public DbSet<Participant> Participants { get; set; } = null!;
    public DbSet<Registration> Registrations { get; set; } = null!;
    public DbSet<Speaker> Speakers { get; set; } = null!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Speaker>().HasData(
            new Speaker { Id = 1, FullName = "Mikolaj Rafalski" },
            new Speaker { Id = 2, FullName = "Rafal Mikolajski" }
        );

        modelBuilder.Entity<Participant>().HasData(
            new Participant { Id = 1, FullName = "Pawel Borys", Email = "pawel@gmail.com" },
            new Participant { Id = 2, FullName = "Andrzej Sapkowski", Email = "andrzej@wp.pl" }
        );

        modelBuilder.Entity<Event>().HasData(
            new Event
            {
                Id = 1,
                Title = "PJATK 2025",
                Description = "Wprowadzamy innowacje!",
                Date = new DateTime(2025, 7, 1, 10, 0, 0, DateTimeKind.Utc),
                MaxParticipants = 100
            },
            new Event
            {
                Id = 2,
                Title = "APBD - dodatkowe zadanie",
                Description = "Nadrabiamy punkty!",
                Date = new DateTime(2025, 7, 5, 12, 0, 0, DateTimeKind.Utc),
                MaxParticipants = 50
            }
        );

        modelBuilder.Entity<EventSpeaker>().HasData(
            new EventSpeaker { EventId = 1, SpeakerId = 1 },
            new EventSpeaker { EventId = 2, SpeakerId = 2 }
        );
        
        modelBuilder.Entity<Registration>().HasData(
            new Registration
            {
                EventId = 1,
                ParticipantId = 1,
                RegistrationDate = DateTime.UtcNow,
                Cancelled = false
            }
        );
    }
    
}