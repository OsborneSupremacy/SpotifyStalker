namespace Spotify.Model;

public record PlaylistModel : Playlist, IApiRequestObject
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

public record PlaylistModelTrack
{
    [JsonPropertyName("track")]
    public Track? Track { get; set; }
}
