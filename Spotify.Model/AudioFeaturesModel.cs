using Spotify.Interface;
using Spotify.Object;

namespace Spotify.Model
{
    public class AudioFeaturesModel : AudioFeatures, IApiRequestObject
    {
        public string UrlTemplate => "audio-features/{Id}";
    }
}
