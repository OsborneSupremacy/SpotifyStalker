using Spotify.Interface;
using Spotify.Object;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Spotify.Model
{
    public class PlaylistModel : Playlist, IApiRequestObject
    {
        [JsonPropertyName("items")]
        public IEnumerable<PlaylistModelTrack> Items { get; set; }

        public string UrlTemplate => "playlists/{Id}/tracks?limit={Limit}";
    }

    public class PlaylistModelTrack
    {
        [JsonPropertyName("track")]
        public Track Track { get; set; }
    }
}
