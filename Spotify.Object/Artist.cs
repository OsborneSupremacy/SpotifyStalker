namespace Spotify.Object;

public record Artist : ISpotifyStandardObject
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("genres")]
    public IEnumerable<string>? Genres { get; set; }

    [JsonPropertyName("popularity")]
    public int Popularity { get; set; }
}
