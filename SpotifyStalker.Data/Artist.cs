using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SpotifyStalker.Data
{
    [Index(nameof(ArtistId))]
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
}
