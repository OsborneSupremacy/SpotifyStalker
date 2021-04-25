using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Spotify.Object
{
    public class UserPlaylistSearchResult
    {
        [JsonPropertyName("playlists")]
        public SearchResultPlaylists Playlists { get; set; }
    }

    public class SearchResultPlaylists
    {
        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("items")]
        public IEnumerable<Playlist> Items { get; set; }
    }
}
