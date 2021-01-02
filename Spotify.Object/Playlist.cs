using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Spotify.Object
{
    public class Playlist
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("tracks")]
        public PlaylistTrackInfo? Tracks { get; set; }

        // populated by app; not returned by Spotify
        public IEnumerable<Track>? FoundTracks { get; set; }

        [JsonPropertyName("owner")]
        public User? Owner { get; set; }
    }

    public class PlaylistTrackInfo
    {
        [JsonPropertyName("href")]
        public string? Href { get; set; }

        [JsonPropertyName("total")]
        public int? Total { get; set; }
    }
}
