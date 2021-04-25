using Spotify.Interface;
using Spotify.Object;

namespace Spotify.Model
{
    public class UserPlaylistSearchResultModel : UserPlaylistSearchResult, IApiRequestObject
    {
        public string UrlTemplate => "search?q=${Id}&type=playlist";
    }
}
