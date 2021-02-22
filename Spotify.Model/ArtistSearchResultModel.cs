using Spotify.Interface;
using Spotify.Object;

namespace Spotify.Model
{
    public class ArtistSearchResultModel : ArtistSearchResult, IApiRequestObject
    {
        public string UrlTemplate => "search?q={Id}&type=artist&limit={Limit}&offset={Offset}";
    }
}
