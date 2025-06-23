using System.ComponentModel.DataAnnotations;

namespace Dodatkowe.DTOs;

public class CreateEventDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public DateTime Date { get; set; }
    public int MaxParticipants { get; set; }
}