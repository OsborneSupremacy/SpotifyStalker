namespace Spotify.Model;

public record UserPlaylistsModel : UserPlaylists, IApiRequestObject
{
    public string UrlTemplate => "users/{Id}/playlists?limit={Limit}";
}
