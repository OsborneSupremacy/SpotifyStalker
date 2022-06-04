using Spotify.Interface;
using Spotify.Object;

namespace Spotify.Model;

public class TrackSearchResultModel : TrackSearchResult, IApiRequestObject
{
    public string UrlTemplate => "artists/{Id}/top-tracks?market=US";
}
