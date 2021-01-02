using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Spotify.Object
{
    public class Artist
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("genres")]
        public IEnumerable<string> Genres { get; set; }

        [JsonPropertyName("tracks")]
        public IEnumerable<Track> Tracks { get; set; }
    }
}
