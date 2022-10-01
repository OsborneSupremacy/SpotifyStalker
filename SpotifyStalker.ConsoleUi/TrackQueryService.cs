﻿namespace SpotifyStalker.ConsoleUi;

public class TrackQueryService
{
    private readonly ILogger<TrackQueryService> _logger;

    private readonly IApiQueryService _apiQueryService;

    private readonly SpotifyStalkerDbContext _dbContext;

    private readonly IDateTimeProvider _dateTimeProvider;

    public TrackQueryService(
        ILogger<TrackQueryService> logger,
        IApiQueryService apiQueryService,
        SpotifyStalkerDbContext dbContext,
        IDateTimeProvider dateTimeProvider
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _apiQueryService = apiQueryService ?? throw new ArgumentNullException(nameof(apiQueryService));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
    }

    public async Task ExecuteAsync()
    {
        _logger.LogDebug("Querying Tracks");

        var artists = _dbContext.Artists
            .OrderByDescending(x => x.Popularity)
            .ThenBy(x => x.ArtistId)
            .ToList();

        _logger.LogDebug($"Querying Tracks for {artists.Count} Artists");

        // begin - Artist loop
        foreach (var artist in artists)
        {
            if (_dbContext.Tracks.Any(x => x.ArtistId == artist.ArtistId))
            {
                _logger.LogInformation("{ArtistName} already queried. Skipping.", artist.ArtistName);
                continue;
            };

            _logger.LogDebug("Querying Tracks for {ArtistName}", artist.ArtistName);

            var trackDictionary = await GetTracksAsync(artist);
            if (!trackDictionary.Any()) continue;

            var result = await _apiQueryService
                .QueryAsync<AudioFeaturesModelCollection>(trackDictionary.Keys);

            await result.Match
            (
                async success =>
                {
                    return await AddAudioFeaturesAsync(artist, trackDictionary, success);
                },
                exception =>
                {
                    _logger.LogInformation("No audio features fround for top tracks of {ArtistId}", artist.ArtistId);
                    return Task.FromResult(true);
                }
            );
        } // end - artist loop
    }

    protected async Task<bool> AddAudioFeaturesAsync(
        Artist artist,
        Dictionary<string, Spotify.Object.Track> trackDictionary,
        AudioFeaturesModelCollection audioFeatures
        )
    {
        if(!audioFeatures.AudioFeaturesList.Any())
        {
            _logger.LogInformation("No audio features fround for top tracks of {ArtistId}", artist.ArtistId);
            return true;
        }

        await _dbContext.Tracks.AddRangeAsync(
            audioFeatures.AudioFeaturesList
                .Where(x => x != null) // ran into Spotify returning a null audio features object
                .Select(x => new Track()
                {
                    Id = x.Id,
                    ArtistId = artist.ArtistId,
                    Name = (trackDictionary.GetValueOrDefault(x.Id)?.Name ?? string.Empty).Left(255),
                    Popularity = trackDictionary.GetValueOrDefault(x.Id)?.Popularity,
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
                    TimeSignature = x.TimeSignature,
                    AddedDate = _dateTimeProvider.GetCurrentDateTime()
                })
                .Distinct()
        );

        await _dbContext.SaveChangesAsync();
        _logger.LogDebug("{ArtistName} tracks saved", artist.ArtistName);
        return true;
    }

    protected async Task<Dictionary<string, Spotify.Object.Track>> GetTracksAsync(Artist artist)
    {
        var result = await _apiQueryService.QueryAsync<TrackSearchResultModel>(artist.ArtistId);

        return result.Match
        (
            success =>
            {
                if(!success.Tracks.Any())
                {
                    _logger.LogInformation("No top tracks fround for {ArtistId}", artist.ArtistId);
                    return new Dictionary<string, Spotify.Object.Track>();
                }
                var existingTrackIds = _dbContext.Tracks
                    .Where(x => success.Tracks.Select(t => t.Id).Contains(x.Id))
                    .Select(x => x.Id)
                    .ToHashSet();

                return success.Tracks
                    .Where(x => x != null)
                    .Where(x => !existingTrackIds.Contains(x.Id))
                    .ToDictionary(x => x.Id, x => x);
            },
            exception =>
            {
                _logger.LogInformation("No top tracks fround for {ArtistId}", artist.ArtistId);
                return new Dictionary<string, Spotify.Object.Track>();
            }
        );

    }
}
