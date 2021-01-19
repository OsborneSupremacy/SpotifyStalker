using Spotify.Model;
using Spotify.Object;
using System.Collections.Concurrent;

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
            AudioFeatures = new CategoryViewModel<AudioFeaturesModel>();
            Metrics = new CategoryViewModel<Metric>();
            Losers = new ConcurrentDictionary<string, AudioFeaturesModel>();
            Winners = new ConcurrentDictionary<string, AudioFeaturesModel>();
        }

        public string UserName { get; set; }

        public RequestStatus UserPlaylistResult { get; set; }

        public CategoryViewModel<PlaylistModel> Playlists { get; set; }

        public CategoryViewModel<ArtistModel> Artists { get; set; }

        public CategoryViewModel<GenreModel> Genres { get; set; }

        public CategoryViewModel<Track> Tracks { get; set; }

        public CategoryViewModel<AudioFeaturesModel> AudioFeatures { get; set; }

        public ConcurrentDictionary<string, AudioFeaturesModel> Losers { get; set; }

        public ConcurrentDictionary<string, AudioFeaturesModel> Winners { get; set; }

        public CategoryViewModel<Metric> Metrics { get; set; }
    }
}
