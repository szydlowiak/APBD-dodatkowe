using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dodatkowe.Models;

[Table("EventSpeakers")]
[PrimaryKey(nameof(EventId), nameof(SpeakerId))]
public class EventSpeaker
{
    [Column("EventId")]
    public int EventId { get; set; }

    [Column("SpeakerId")]
    public int SpeakerId { get; set; }
    
    [ForeignKey(nameof(EventId))]
    public Event Event { get; set; }
    
    [ForeignKey(nameof(SpeakerId))]
    public Speaker Speaker { get; set; }
}