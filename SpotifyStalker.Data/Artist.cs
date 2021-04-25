using System.ComponentModel.DataAnnotations;

namespace SpotifyStalker.Data
{
    public record Artist
    {
        [Required]
        [MaxLength(255)]
        public string ArtistId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ArtistName { get; set; }

        [Required]
        public int Popularity { get; set; }

        [Required]
        [MaxLength(2040)]
        public string Genres { get; set; }
    }
}
