namespace Spotify.Model;

public record TrackSearchResultModel : TrackSearchResult, IApiRequestObject
{
    public string UrlTemplate => "artists/{Id}/top-tracks?market=US";
}
