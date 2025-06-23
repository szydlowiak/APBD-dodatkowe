namespace Dodatkowe.DTOs;

public class UpcomingEventDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public int MaxParticipants { get; set; }
    public int RegisteredParticipants { get; set; }
    public int FreeSpots => MaxParticipants - RegisteredParticipants;
    public List<SpeakerDto> Speakers { get; set; } = new();
}

public class SpeakerDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
}