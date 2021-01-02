using Spotify.Model;
using Spotify.Object;
using SpotifyStalker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyStalker2.ViewModel
{
    public class StalkerViewModel
    {
        public StalkerViewModel() {
        }

        public RequestStatus UserPlaylistResult { get; set; }

        public UserPlaylistsModel UserPlaylistsModel { get; set; }

        public List<Playlist> GetOrderedPlayLists() =>
            UserPlaylistsModel
                .Playlists
                .OrderByDescending(x => x?.Tracks?.Total ?? 0)
                .ToList();

        public string UserName { get; set; }

        public int PlaylistCount => 
            UserPlaylistsModel?.Playlists?.Count() ?? 0;

        public int ProcessedPlaylistCount => _processedPlaylistCount;
        protected int _processedPlaylistCount = 0;

        public bool ShowPlayLists =>
            UserPlaylistsModel?.Playlists?.Any() ?? false;

        public bool PlaylistsProcessing =>
            ProcessedPlaylistCount < PlaylistCount;

        public void ResetProcessedPlaylistCount() => _processedPlaylistCount = 0;

        public void IncrementProcessedPlaylistCount() => 
            Interlocked.Increment(ref _processedPlaylistCount);
    }
}
