﻿using Spotify.Object;
using SpotifyStalker.Model;

namespace SpotifyStalker.Interface
{
    public interface IStalkModelTransformer
    {
        StalkModel IncrementProcessedPlaylistCount(StalkModel stalkModel);

        StalkModel RegisterTrack(StalkModel stalkModel, Track track);

        StalkModel Reset(StalkModel stalkModel);

        StalkModel BeginProcessing<T>(StalkModel stalkModel);

        StalkModel EndProcessing<T>(StalkModel stalkModel);
    }
}
