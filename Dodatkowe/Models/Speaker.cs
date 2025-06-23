using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dodatkowe.Models;

[Table("Speakers")]
public class Speaker
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(150)]
    public string FullName { get; set; }

    public ICollection<EventSpeaker> EventSpeakers { get; set; }
}