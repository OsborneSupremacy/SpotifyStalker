using Spotify.Model;
using Spotify.Object;
using SpotifyStalker.Model;
using System;
using System.Collections.Concurrent;
using AutoMapper;
using SpotifyStalker.Interface;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Spotify.Interface;

namespace SpotifyStalker.Service
{
    public class StalkModelTransformer : IStalkModelTransformer
    {
        private readonly IMapper _mapper;

        private readonly ILogger<IApiRequestService> _logger;

        public StalkModelTransformer(
            ILogger<IApiRequestService> logger,
            IMapper mapper
        ) {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public StalkModel Reset(StalkModel stalkModel)
        {
            stalkModel.UserPlaylistResult = RequestStatus.Default;

            stalkModel.Playlists = new CategoryViewModel<PlaylistModel>();
            stalkModel.Artists = new CategoryViewModel<ArtistModel>();
            stalkModel.Genres = new CategoryViewModel<GenreModel>();
            stalkModel.Tracks = new CategoryViewModel<Track>();

            return stalkModel;
        }

        protected CategoryViewModel<T> GetCategoryViewModel<T>(StalkModel stalkModel) where T : ISpotifyStandardObject => 
            typeof(T).Name switch
            {
                nameof(PlaylistModel) => stalkModel.Playlists as CategoryViewModel<T>,
                nameof(ArtistModel) => stalkModel.Artists as CategoryViewModel<T>,
                nameof(GenreModel) => stalkModel.Genres as CategoryViewModel<T>,
                nameof(Track) => stalkModel.Tracks as CategoryViewModel<T>,
                _ => throw new NotSupportedException($"Type `{typeof(T).Name}` not supported")
            };

        public StalkModel IncrementCount<T>(StalkModel stalkModel) where T : ISpotifyStandardObject
        {
            var categoryViewModel = GetCategoryViewModel<T>(stalkModel);
            categoryViewModel.Processed++;
            return stalkModel;
        }

        public StalkModel BeginProcessing<T>(StalkModel stalkModel) where T : ISpotifyStandardObject =>
            UpdateProcessing<T>(stalkModel, true);

        public StalkModel EndProcessing<T>(StalkModel stalkModel) where T : ISpotifyStandardObject => 
            UpdateProcessing<T>(stalkModel, false);

        protected StalkModel UpdateProcessing<T>(StalkModel stalkModel, bool processing) where T : ISpotifyStandardObject
        {
            var categoryViewModel = GetCategoryViewModel<T>(stalkModel);
            categoryViewModel.InProcess = processing;
            return stalkModel;
        }

        public List<T> GetOrderedItems<T>(StalkModel stalkModel) where T : ISpotifyStandardObject =>
            GetCategoryViewModel<T>(stalkModel).GetOrderedItems();

        public StalkModel RegisterPlaylists(StalkModel stalkModel, IEnumerable<Playlist> playlists)
        {
            if (!playlists?.Any() ?? false) return stalkModel;

            foreach(var playlist in playlists)
                stalkModel.Playlists.TryAdd(playlist.Id, _mapper.Map<PlaylistModel>(playlist));

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
                    stalkModel.Artists.Items[artistId].Tracks = new ConcurrentDictionary<string, Track>();
                };

                stalkModel.Artists.Items[artistId].Tracks.TryAdd(trackId, track);
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
                    stalkModel.Genres.Items[genre].Artists = new ConcurrentDictionary<string, ArtistModel>();
                    stalkModel.Genres.Items[genre].Tracks = new ConcurrentDictionary<string, Track>();
                    stalkModel.Genres.Processed++;
                }

                stalkModel.Genres.Items[genre].Artists.TryAdd(artist.Id, artist);

                if (artist.Tracks == null) continue;

                foreach(var track in artist.Tracks)
                    stalkModel.Genres.Items[genre].Tracks.TryAdd(track.Key, track.Value);
            }
            return stalkModel;
        }
    }
}
