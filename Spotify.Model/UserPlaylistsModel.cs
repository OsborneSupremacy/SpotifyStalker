using Spotify.Interface;
using Spotify.Object;

namespace Spotify.Model
{
    public class UserPlaylistsModel : UserPlaylists, IApiRequestObject
    {
        public string UrlTemplate => "users/{Id}/playlists?limit={Limit}";
    }
}
