using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace SpotifyStalker.Data
{
    [Index(nameof(ArtistId))]
    [Index(nameof(Name))]
    public record Track
    {
        [Key]
        [Required]
        [MaxLength(255)]
        public string Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string ArtistId { get; set; }

        [Required]
        [MaxLength(255)] public string Name { get; set; }

        public double? Popularity { get; set; }

        public double? Danceability { get; set; }

        public double? Energy { get; set; }

        public double? Key { get; set; }

        public double? Loudness { get; set; }

        public double? Mode { get; set; }

        public double? Speechiness { get; set; }

        public double? Acousticness { get; set; }

        public double? Instrumentalness { get; set; }

        public double? Liveness { get; set; }

        public double? Valence { get; set; }

        public double? Tempo { get; set; }

        public double? DurationMs { get; set; }

        public double? TimeSignature { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
