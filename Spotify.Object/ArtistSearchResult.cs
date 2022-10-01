namespace Spotify.Object;

public record ArtistSearchResult
{
    [JsonPropertyName("artists")]
    public SearchResultArtists? Artists { get; set; }
}

public record SearchResultArtists
{
    [JsonPropertyName("items")]
    public IEnumerable<Artist>? Items { get; set; }

    [JsonPropertyName("next")]
    public string? Next { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
}
