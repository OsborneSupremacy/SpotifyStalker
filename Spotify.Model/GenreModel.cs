using Spotify.Object;
using System.Collections.Concurrent;

namespace Spotify.Model
{
    public class GenreModel
    {
        public string Name { get; set; }

        public ConcurrentDictionary<string, ArtistModel> Artists { get; set; }

        public ConcurrentDictionary<string, Track> Tracks { get; set; }
    }
}
