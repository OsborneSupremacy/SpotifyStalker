namespace Spotify.Object;

public record TrackSearchResult
{
    [JsonPropertyName("tracks")]
    public IEnumerable<Track>? Tracks { get; set; }
}
