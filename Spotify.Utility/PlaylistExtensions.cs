using Spotify.Object;

namespace Spotify.Utility;

public static class PlaylistExtensions
{
    public static bool IsOwnedBySpotify(this Playlist input) =>
        input?.Owner?.Id?
            .Equals("spotify", System.StringComparison.OrdinalIgnoreCase) ?? true;
}
