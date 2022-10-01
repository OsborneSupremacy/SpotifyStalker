namespace Spotify.Object;

public record UserPlaylistSearchResult
{
    [JsonPropertyName("playlists")]
    public SearchResultPlaylists? Playlists { get; set; }
}

public record SearchResultPlaylists
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    [JsonPropertyName("items")]
    public IEnumerable<Playlist>? Items { get; set; }
}
