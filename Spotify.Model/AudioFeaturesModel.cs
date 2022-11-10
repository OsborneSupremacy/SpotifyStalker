namespace Spotify.Model;

public record AudioFeaturesModel : AudioFeatures, IApiRequestObject, ISpotifyStandardObject
{
    public string UrlTemplate => "audio-features/{Id}";

    // these objects don't have a separate name, so just use ID as name (it won't be displayed anywhere)
    public string Name => Id ?? string.Empty;
}

public record AudioFeaturesModelCollection : IApiBatchRequestObject, IApiRequestObject
{
    [JsonIgnore]
    public string UrlBase => "audio-features?ids=";

    [JsonIgnore]
    public string UrlTemplate => "audio-features?ids={Ids}";

    [JsonPropertyName("audio_features")]
    public IEnumerable<AudioFeaturesModel>? AudioFeaturesList { get; set; }
}
