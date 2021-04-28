using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace SpotifyStalker.Data
{
    [Index(nameof(SearchTerm))]
    public class ArtistQueryLog
    {
        [Key]
        [Required]
        [MaxLength(255)]
        public string SearchTerm { get; set; }

        [Required]
        public DateTime QueriedDate { get; set; }

        public int? ResultCount { get; set; }

        public DateTime? CompletedDate { get; set; }
    }
}
