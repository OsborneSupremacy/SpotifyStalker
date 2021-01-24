using Spotify.Interface;
using Spotify.Object;

namespace Spotify.Model
{
    public class SearchResultModel : SearchResult, IApiRequestObject
    {
        public string UrlTemplate => "search?q=${Id}&type=playlist";
    }
}
