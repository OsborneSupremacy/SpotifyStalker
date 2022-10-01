namespace Spotify.Model;

public record ArtistSearchResultModel : ArtistSearchResult, IApiRequestObject
{
    public string UrlTemplate => "search?q={Id}&type=artist&limit={Limit}&offset={Offset}";
}
