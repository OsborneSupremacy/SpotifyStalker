using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Spotify.Interface;
using Spotify.Object;

namespace Spotify.Model;

public class PlaylistModel : Playlist, IApiRequestObject
{
    public PlaylistModel()
    {
        Tracks = new ConcurrentDictionary<string, Track>();
    }

    [JsonPropertyName("items")]
    public IEnumerable<PlaylistModelTrack> Items { get; set; }

    public ConcurrentDictionary<string, Track>? Tracks { get; set; }

    public string UrlTemplate => "playlists/{Id}/tracks?limit={Limit}";
}

public class PlaylistModelTrack
{
    [JsonPropertyName("track")]
    public Track? Track { get; set; }
}
