using Spotify.Interface;
using System.Text.Json.Serialization;

namespace Spotify.Object
{
    public class Playlist : ISpotifyStandardObject
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        // Spotify's name for this property, "Tracks", is potentially misleading.
        // The property is not a list of tracks, but a single object containing metadata.
        [JsonPropertyName("tracks")]
        public PlaylistTrackInfo? TrackInfo { get; set; }

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
