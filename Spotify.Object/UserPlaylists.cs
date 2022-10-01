namespace Spotify.Object;

public record UserPlaylists
{
    [JsonPropertyName("items")]
    public IEnumerable<Playlist>? Playlists { get; set; }
}
