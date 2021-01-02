using Spotify.Interface;
using Spotify.Object;

namespace Spotify.Model
{
    public class ArtistModel : Artist, IApiRequestObject
    {
        public string UrlTemplate => "artists/{Id}";
    }
}
