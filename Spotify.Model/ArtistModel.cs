namespace Spotify.Model;

public record ArtistModel : Artist, IApiRequestObject
{
    [JsonIgnore]
    public string UrlTemplate => "artists/{Id}";

    public ConcurrentDictionary<string, Track>? Tracks { get; set; }
}

public record ArtistModelCollection : IApiBatchRequestObject
{
    [JsonIgnore]
    public string UrlBase => "artists?ids=";

    [JsonPropertyName("artists")]
    public IEnumerable<ArtistModel>? Artists { get; set; }
}
