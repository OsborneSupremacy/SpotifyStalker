using Spotify.Interface;
using Spotify.Object;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Spotify.Model
{
    public class AudioFeaturesModel : AudioFeatures, IApiRequestObject, ISpotifyStandardObject
    {
        public string UrlTemplate => "audio-features/{Id}";

        // these objects don't have a separate name, so just use ID as name (it won't be displayed anywhere)
        public string Name => Id;
    }

    public class AudioFeaturesModelCollection : IApiBatchRequestObject
    {
        [JsonIgnore]
        public string UrlBatch => "audio-features?ids=";

        [JsonPropertyName("audio_features")]
        public IEnumerable<AudioFeaturesModel> AudioFeaturesList { get; set; }
    }
}
