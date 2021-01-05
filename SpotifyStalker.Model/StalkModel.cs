using Spotify.Model;
using Spotify.Object;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SpotifyStalker.Model
{
    public class StalkModel
    {
        public StalkModel()
        {
            Playlists = new CategoryViewModel<PlaylistModel>();
            Artists = new CategoryViewModel<ArtistModel>();
            Genres = new CategoryViewModel<GenreModel>();
            Tracks = new CategoryViewModel<Track>();
        }

        public string UserName { get; set; }

        public RequestStatus UserPlaylistResult { get; set; }

        public CategoryViewModel<PlaylistModel> Playlists { get; set; }

        public CategoryViewModel<ArtistModel> Artists { get; set; }

        public CategoryViewModel<GenreModel> Genres { get; set; }

        public CategoryViewModel<Track> Tracks { get; set; }


        //public List<ArtistModel> GetOrderedArtists() =>
        //    Artists
        //        .OrderByDescending(x => x.Value?.Tracks.Count() ?? 0)
        //        .Select(x => x.Value)
        //        .ToList();

        //public List<GenreModel> GetOrderedGenres() =>
        //    Genres
        //        .OrderByDescending(x => x.Value?.Tracks.Count() ?? 0)
        //        .Select(x => x.Value)
        //        .ToList();
    }
}
