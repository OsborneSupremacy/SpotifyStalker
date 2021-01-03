using Spotify.Model;
using Spotify.Object;
using SpotifyStalker.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SpotifyStalker.Interface;

namespace SpotifyStalker.Service
{
    public class StalkModelTransformer : IStalkModelTransformer
    {
        private readonly IMapper _mapper;

        public StalkModelTransformer(IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public StalkModel Reset(StalkModel stalkModel)
        {
            stalkModel.ProcessedPlaylistCount = 0;

            stalkModel.Artists = new ConcurrentDictionary<string, ArtistModel>();
            stalkModel.Tracks = new ConcurrentDictionary<string, Track>();

            stalkModel.UserPlaylistsModel = null;
            stalkModel.UserPlaylistResult = RequestStatus.Default;

            stalkModel.TracksProcessing = false;
            stalkModel.ArtistsProcessing = false;

            return stalkModel;
        }

        public StalkModel BeginProcessing<T>(StalkModel stalkModel)
        {
            if(typeof(T) == typeof(Track))
                stalkModel.TracksProcessing = true;

            if (typeof(T) == typeof(Artist))
                stalkModel.ArtistsProcessing = true;

            return stalkModel;
        }

        public StalkModel EndProcessing<T>(StalkModel stalkModel)
        {
            if (typeof(T) == typeof(Track))
                stalkModel.TracksProcessing = false;

            if (typeof(T) == typeof(Artist))
                stalkModel.ArtistsProcessing = false;

            return stalkModel;
        }

        public StalkModel RegisterTrack(
            StalkModel stalkModel,
            Track track
        )
        {
            string trackId = track.Id ?? track.Name.ToLowerInvariant();

            if (!stalkModel.Tracks.TryAdd(trackId, track))
                return stalkModel; // if track wasn't added now, it was already added, so don't need to do anything more.

            foreach (var artist in track.Artists)
            {
                var artistId = artist.Id ?? artist.Name.ToLowerInvariant();

                if (stalkModel.Artists.TryAdd(artistId, _mapper.Map<ArtistModel>(artist)))
                {
                    // if artists was just added now, instantiate their track list and
                    stalkModel.Artists[artistId].Tracks = new ConcurrentDictionary<string, Track>();
                };

                stalkModel.Artists[artistId].Tracks.TryAdd(trackId, track);
            }
            return stalkModel;
        }

        public StalkModel IncrementProcessedPlaylistCount(
            StalkModel stalkModel
            )
        {
            stalkModel.ProcessedPlaylistCount++;
            return stalkModel;
        }

    }
}
