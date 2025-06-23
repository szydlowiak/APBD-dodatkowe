using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dodatkowe.Models;

[Table("Registrations")]
[PrimaryKey(nameof(EventId), nameof(ParticipantId))]
public class Registration
{
    [Column("EventId")]
    public int EventId { get; set; }

    [Column("ParticipantId")]
    public int ParticipantId { get; set; }

    public DateTime RegistrationDate { get; set; }

    public bool Cancelled { get; set; }

    [ForeignKey(nameof(EventId))]
    public virtual Event Event { get; set; } = null!;

    [ForeignKey(nameof(ParticipantId))]
    public virtual Participant Participant { get; set; } = null!;
}