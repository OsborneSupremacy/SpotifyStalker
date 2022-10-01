namespace Spotify.Model;

public record UserPlaylistSearchResultModel : UserPlaylistSearchResult, IApiRequestObject
{
    public string UrlTemplate => "search?q=${Id}&type=playlist";
}
