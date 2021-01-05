using Spotify.Interface;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Spotify.Object
{
    public class Track : ISpotifyStandardObject
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("popularity")]
        public double? Popularity { get; set; }

        [JsonPropertyName("album")]
        public Album Album { get; set; }

        [JsonPropertyName("audioFeatures")]
        public AudioFeatures AudioFeatures { get; set; }

        [JsonPropertyName("artists")]
        public IEnumerable<Artist> Artists { get; set; }
    }
}
