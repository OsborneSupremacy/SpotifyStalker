using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SpotifyStalker.Data;

[Index(nameof(ArtistId))]
[Index(nameof(ArtistName))]
public record Artist
{
    [Key]
    [Required]
    [MaxLength(255)]
    public string ArtistId { get; set; }

    [Required]
    [MaxLength(255)]
    public string ArtistName { get; set; }

    [Required]
    public int Popularity { get; set; }

    [Required]
    [MaxLength(4080)]
    public string Genres { get; set; }
}
