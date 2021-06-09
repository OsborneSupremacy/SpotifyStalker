using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Spotify.Object
{
    public class TrackSearchResult
    {
        [JsonPropertyName("tracks")]
        public IEnumerable<Track> Tracks { get; set; }
    }
}
