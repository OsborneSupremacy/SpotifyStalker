using Spotify.Model;
using Spotify.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyStalker2.ViewModel
{
    public class StalkerViewModel
    {
        public UserPlaylistsModel UserPlaylistsModel { get; set; }

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

    }
}
