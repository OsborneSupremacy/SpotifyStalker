using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Spotify.Object
{
    public class ArtistSearchResult
    {
        [JsonPropertyName("artists")]
        public SearchResultArtists? Artists { get; set; }
    }

    public class SearchResultArtists
    {
        [JsonPropertyName("items")]
        public IEnumerable<Artist>? Items { get; set; }

        [JsonPropertyName("next")]
        public string? Next { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}
