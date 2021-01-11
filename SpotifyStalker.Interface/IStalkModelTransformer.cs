using Spotify.Interface;
using Spotify.Model;
using Spotify.Object;
using SpotifyStalker.Model;
using System.Collections.Generic;

namespace SpotifyStalker.Interface
{
    public interface IStalkModelTransformer
    {
        StalkModel RegisterTrack(StalkModel stalkModel, PlaylistModel playlistModel, Track track);

        StalkModel RegisterGenre(StalkModel stalkModel, ArtistModel artist);

        StalkModel RegisterAudioFeature(StalkModel stalk, AudioFeaturesModel audioFeatures);

        StalkModel Reset(StalkModel stalkModel);

        StalkModel BeginProcessing<T>(StalkModel stalkModel) where T : ISpotifyStandardObject;

        StalkModel EndProcessing<T>(StalkModel stalkModel) where T : ISpotifyStandardObject;

        StalkModel IncrementCount<T>(StalkModel stalkModel) where T : ISpotifyStandardObject;

        List<T> GetItems<T>(StalkModel stalkModel) where T : ISpotifyStandardObject;

        StalkModel RegisterPlaylists(StalkModel stalkModel, IEnumerable<Playlist> playlists);
    }
}
