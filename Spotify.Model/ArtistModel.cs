using Spotify.Interface;
using Spotify.Object;
using System.Collections.Concurrent;

namespace Spotify.Model
{
    public class ArtistModel : Artist, IApiRequestObject
    {
        public string UrlTemplate => "artists/{Id}";

        public ConcurrentDictionary<string, Track>? Tracks { get; set; }
    }
}
