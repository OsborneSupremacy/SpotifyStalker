using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Spotify.Object
{
    public class UserPlaylists
    {
        [JsonPropertyName("items")]
        public IEnumerable<Playlist>? Playlists { get; set; }
    }
}
