using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dodatkowe.Models;

[Table("Events")]
public class Event
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public int MaxParticipants { get; set; }

    public ICollection<EventSpeaker> EventSpeakers { get; set; } = new List<EventSpeaker>();
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}