using Spotify.Interface;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Spotify.Object
{
    public class Artist : ISpotifyStandardObject
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("genres")]
        public IEnumerable<string> Genres { get; set; }

        [JsonPropertyName("popularity")]
        public int Popularity { get; set; }
    }
}
