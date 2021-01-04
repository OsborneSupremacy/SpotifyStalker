using Spotify.Model;
using Spotify.Object;
using SpotifyStalker.Model;

namespace SpotifyStalker.Interface
{
    public interface IStalkModelTransformer
    {
        StalkModel IncrementProcessedPlaylistCount(StalkModel stalkModel);

        StalkModel IncrementProcessedGenreCount(StalkModel stalkModel);

        StalkModel RegisterTrack(StalkModel stalkModel, Track track);

        StalkModel RegisterGenre(StalkModel stalkModel, ArtistModel artist);

        StalkModel Reset(StalkModel stalkModel);

        StalkModel BeginProcessing<T>(StalkModel stalkModel);

        StalkModel EndProcessing<T>(StalkModel stalkModel);
    }
}
