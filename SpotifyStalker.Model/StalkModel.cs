using Spotify.Model;
using Spotify.Object;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SpotifyStalker.Model
{
    public class StalkModel
    {
        public RequestStatus UserPlaylistResult { get; set; }

        public UserPlaylistsModel UserPlaylistsModel { get; set; }

        public ConcurrentDictionary<string, ArtistModel> Artists;

        public ConcurrentDictionary<string, Track> Tracks;

        public ConcurrentDictionary<string, GenreModel> Genres;

        public bool TracksProcessing;

        public bool ArtistsProcessing;

        public bool GenresProcessing;

        public List<Playlist> GetOrderedPlayLists() =>
            UserPlaylistsModel
                .Playlists
                .OrderByDescending(x => x?.Tracks?.Total ?? 0)
                .ToList();

        public string UserName { get; set; }

        public int PlaylistCount =>
            UserPlaylistsModel?.Playlists?.Count() ?? 0;

        public int ProcessedPlaylistCount { get; set; }

        public bool ShowPlayLists =>
            UserPlaylistsModel?.Playlists?.Any() ?? false;

        public bool PlaylistsProcessing =>
            ProcessedPlaylistCount < PlaylistCount;

        public bool ShowArtists => Artists?.Any() ?? false;

        public int ArtistCount =>
            Artists?.Count() ?? 0;

        public int GenreCount => Genres?.Count() ?? 0;

        public int ProcessedGenreCount { get; set; }

        public List<ArtistModel> GetOrderedArtists() =>
            Artists
                .OrderByDescending(x => x.Value?.Tracks.Count() ?? 0)
                .Select(x => x.Value)
                .ToList();

        public List<GenreModel> GetOrderedGenres() =>
            Genres
                .OrderByDescending(x => x.Value?.Tracks.Count() ?? 0)
                .Select(x => x.Value)
                .ToList();
    }
}
