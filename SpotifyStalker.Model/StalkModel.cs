using Spotify.Model;
using Spotify.Object;

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
        }

        public (bool InProcess, string Stage) Processing { get; set; }

        public string UserName { get; set; }

        public RequestStatus UserPlaylistResult { get; set; }

        public CategoryViewModel<PlaylistModel> Playlists { get; set; }

        public CategoryViewModel<ArtistModel> Artists { get; set; }

        public CategoryViewModel<GenreModel> Genres { get; set; }

        public CategoryViewModel<Track> Tracks { get; set; }

        public CategoryViewModel<AudioFeaturesModel> AudioFeatures { get; set; }

        public CategoryViewModel<Metric> Metrics { get; set; }
    }
}
