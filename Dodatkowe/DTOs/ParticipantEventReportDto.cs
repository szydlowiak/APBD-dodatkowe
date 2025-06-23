namespace Dodatkowe.DTOs;

public class ParticipantEventReportDto
{
    public string EventTitle { get; set; }
    public DateTime Date { get; set; }
    public List<SpeakerDto> Speakers { get; set; } = new();
}