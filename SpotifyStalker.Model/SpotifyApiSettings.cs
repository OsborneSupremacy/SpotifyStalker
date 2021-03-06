﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Spotify.Model
{
    public class SpotifyApiSettings
    {
        [JsonPropertyName("tokenUrl")]
        [Required(AllowEmptyStrings = false)]
        [Url]
        public string TokenUrl { get; set; }

        [JsonPropertyName("spotifyBaseUrl")]
        [Required(AllowEmptyStrings = false)]
        [Url]
        public string SpotifyBaseUrl { get; set; }

        [JsonPropertyName("apikeys")]
        public ApiKeys ApiKeys { get; set; }

        [JsonPropertyName("limits")]
        [Required]
        public ApiLimits Limits { get; set; }
    }

    public class ApiLimits
    {
        [JsonPropertyName("search")]
        [Required]
        public SearchLimits Search { get; set; }

        [JsonPropertyName("userplaylist")]
        [Required]
        [Range(1, 50)]
        public int UserPlaylist { get; set; }

        [JsonPropertyName("playlisttrack")]
        [Required]
        [Range(1, 100)]
        public int PlaylistTrack { get; set; }

        [JsonPropertyName("batchsize")]
        [Required]
        [Range(1, 50)]
        public int BatchSize { get; set; }
    }

    public class SearchLimits
    {
        [JsonPropertyName("limit")]
        [Required]
        [Range(1, 50)]
        public int Limit { get; set; }

        [JsonPropertyName("maximumoffset")]
        [Required]
        [Range(1, 1000)]
        public int MaximumOffset { get; set; }
    }

    public class ApiKeys
    {
        [JsonPropertyName("clientid")]
        public string ClientId { get; set; }

        [JsonPropertyName("clientsecret")]
        public string ClientSecret { get; set; }
    }
}
