using System.Collections.Generic;
using System.Text.Json.Serialization;
using Spotify.Interface;
using Spotify.Object;

namespace Spotify.Model;

public class AudioFeaturesModel : AudioFeatures, IApiRequestObject, ISpotifyStandardObject
{
    public string UrlTemplate => "audio-features/{Id}";

    // these objects don't have a separate name, so just use ID as name (it won't be displayed anywhere)
    public string Name => Id;
}

public class AudioFeaturesModelCollection : IApiBatchRequestObject, IApiRequestObject
{
    [JsonIgnore]
    public string UrlBase => "audio-features?ids=";

    [JsonIgnore]
    public string UrlTemplate => "audio-features?ids={Ids}";

    [JsonPropertyName("audio_features")]
    public IEnumerable<AudioFeaturesModel>? AudioFeaturesList { get; set; }
}
