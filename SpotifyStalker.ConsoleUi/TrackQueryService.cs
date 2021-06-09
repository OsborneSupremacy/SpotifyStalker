using Microsoft.Extensions.Logging;
using Spotify.Model;
using SpotifyStalker.Data;
using SpotifyStalker.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyStalker.ConsoleUi
{
    public class TrackQueryService
    {
        private readonly ILogger<TrackQueryService> _logger;

        private readonly IApiQueryService _apiQueryService;

        private readonly SpotifyStalkerDbContext _dbContext;

        public TrackQueryService(
            ILogger<TrackQueryService> logger,
            IApiQueryService apiQueryService,
            SpotifyStalkerDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _apiQueryService = apiQueryService ?? throw new ArgumentNullException(nameof(apiQueryService));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task ExecuteAsync()
        {
            _logger.LogDebug("Querying Tracks");

            var artists = _dbContext.Artists
                .OrderBy(x => x.Popularity)
                .ThenBy(x => x.ArtistId)
                .ToList();

            _logger.LogDebug($"Querying Tracks for {artists.Count} Artists");

            // begin - Artist loop
            foreach (var artist in artists) 
            {
                _logger.LogDebug("Querying Tracks for {ArtistName}", artist.ArtistName);

                // we already have tracks for this artist, so skip them
                if (_dbContext.Tracks.Where(x => x.ArtistId == artist.ArtistId).Any()) {
                    _logger.LogInformation("Tracks exist in database for {ArtistId} already. Skipping", artist.ArtistId);
                    continue;
                }

                var (tracksResult, tracks) = await _apiQueryService.QueryAsync<TrackSearchResultModel>(artist.ArtistId);
                if (tracksResult != Model.RequestStatus.Success
                    || !(tracks?.Tracks.Any() ?? false))
                {
                    _logger.LogInformation("No top tracks fround for {ArtistId}", artist.ArtistId);
                    continue;
                }

                var trackDictionary = tracks.Tracks.ToDictionary(x => x.Id, x => x);

                var (audioFeaturesResult, audioFeatures) = await _apiQueryService.QueryAsync<AudioFeaturesModelCollection>(trackDictionary.Keys);
                if (audioFeaturesResult != Model.RequestStatus.Success
                    || !(audioFeatures.AudioFeaturesList.Any())) {
                    _logger.LogInformation("No audio features fround for top tracks of {ArtistId}", artist.ArtistId);
                    continue;
                }

                await _dbContext.Tracks.AddRangeAsync(
                    audioFeatures.AudioFeaturesList.Select(x => new Track() 
                    {
                        Id = x.Id,
                        ArtistId = artist.ArtistId,
                        Name = trackDictionary.GetValueOrDefault(x.Id).Name,
                        Popularity = trackDictionary.GetValueOrDefault(x.Id).Popularity,
                        Danceability = x.Danceability,
                        Energy = x.Energy,
                        Key = x.Key,
                        Loudness = x.Loudness,
                        Mode = x.Mode,
                        Speechiness = x.Speechiness,
                        Acousticness = x.Acousticness,
                        Instrumentalness = x.Instrumentalness,
                        Liveness = x.Liveness,
                        Valence = x.Valence,
                        Tempo = x.Tempo,
                        DurationMs = x.DurationMs,
                        TimeSignature = x.TimeSignature
                    })
                );

                await _dbContext.SaveChangesAsync();

                _logger.LogDebug("{ArtistName} tracks saved", artist.ArtistName);


            } // end - artlist loop

        }

    }
}
