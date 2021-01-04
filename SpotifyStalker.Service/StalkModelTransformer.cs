using Spotify.Model;
using Spotify.Object;
using SpotifyStalker.Model;
using System;
using System.Collections.Concurrent;
using AutoMapper;
using SpotifyStalker.Interface;
using Microsoft.Extensions.Logging;

namespace SpotifyStalker.Service
{
    public class StalkModelTransformer : IStalkModelTransformer
    {
        private readonly IMapper _mapper;

        private readonly ILogger<IApiRequestService> _logger;

        public StalkModelTransformer(
            ILogger<IApiRequestService> logger,
            IMapper mapper
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public StalkModel Reset(StalkModel stalkModel)
        {
            stalkModel.ProcessedPlaylistCount = 0;
            stalkModel.ProcessedGenreCount = 0;

            stalkModel.Artists = new ConcurrentDictionary<string, ArtistModel>();
            stalkModel.Tracks = new ConcurrentDictionary<string, Track>();
            stalkModel.Genres = new ConcurrentDictionary<string, GenreModel>();

            stalkModel.UserPlaylistsModel = null;
            stalkModel.UserPlaylistResult = RequestStatus.Default;

            stalkModel.TracksProcessing = false;
            stalkModel.ArtistsProcessing = false;
            stalkModel.GenresProcessing = false;

            return stalkModel;
        }

        public StalkModel BeginProcessing<T>(StalkModel stalkModel) => UpdateProcessing<T>(stalkModel, true);

        public StalkModel EndProcessing<T>(StalkModel stalkModel) => UpdateProcessing<T>(stalkModel, false);

        protected StalkModel UpdateProcessing<T>(StalkModel stalkModel, bool processing)
        {
            if (typeof(T) == typeof(Track)) stalkModel.TracksProcessing = processing;
            if (typeof(T) == typeof(Artist)) stalkModel.ArtistsProcessing = processing;
            if (typeof(T) == typeof(GenreModel)) stalkModel.GenresProcessing = processing;
            return stalkModel;
        }

        public StalkModel RegisterTrack(
            StalkModel stalkModel,
            Track track
        )
        {
            if(track == null) {
                _logger.LogWarning("Track is null. Skipping");
                return stalkModel;
            }

            string trackId = track.Id ?? track.Name.ToLowerInvariant();

            if (!stalkModel.Tracks.TryAdd(trackId, track))
                return stalkModel; // if track wasn't added now, it was already added, so don't need to do anything more.

            foreach (var artist in track.Artists)
            {
                var artistId = artist.Id ?? artist.Name.ToLowerInvariant();

                if (stalkModel.Artists.TryAdd(artistId, _mapper.Map<ArtistModel>(artist)))
                {
                    // if artist was just added now, instantiate their track list
                    stalkModel.Artists[artistId].Tracks = new ConcurrentDictionary<string, Track>();
                };

                stalkModel.Artists[artistId].Tracks.TryAdd(trackId, track);
            }
            return stalkModel;
        }

        public StalkModel RegisterGenre(StalkModel stalkModel, ArtistModel artist)
        {
            foreach(var genre in artist.Genres)
            {
                if (stalkModel.Genres.TryAdd(genre, new GenreModel() { Name = genre }))
                {
                    // if genre was just added now, instantiate its lists
                    stalkModel.Genres[genre].Artists = new ConcurrentDictionary<string, ArtistModel>();
                    stalkModel.Genres[genre].Tracks = new ConcurrentDictionary<string, Track>();
                    stalkModel.ProcessedGenreCount++;
                }

                stalkModel.Genres[genre].Artists.TryAdd(artist.Id, artist);

                if (artist.Tracks == null) continue;

                foreach(var track in artist.Tracks)
                    stalkModel.Genres[genre].Tracks.TryAdd(track.Key, track.Value);
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

        public StalkModel IncrementProcessedGenreCount(StalkModel stalkModel)
        {
            stalkModel.ProcessedGenreCount++;
            return stalkModel;
        }
    }
}
