using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Spotify.Object
{
    public class Genre
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("artists")]
        public IEnumerable<Artist> Artists { get; set; }

        [JsonPropertyName("tracks")]
        public IEnumerable<Track> Tracks { get; set; }
    }
}
