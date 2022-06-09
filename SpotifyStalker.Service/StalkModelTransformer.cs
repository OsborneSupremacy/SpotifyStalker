using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Spotify.Interface;
using Spotify.Model;
using Spotify.Object;
using SpotifyStalker.Interface;
using SpotifyStalker.Model;

namespace SpotifyStalker.Service;

public class StalkModelTransformer : IStalkModelTransformer
{
    private readonly IMapper _mapper;

    private readonly ILogger<IApiRequestService> _logger;

    private readonly IMetricProvider _metricProvider;

    private readonly double _plotAreaWidth = 931.0;

    public StalkModelTransformer(
        ILogger<IApiRequestService> logger,
        IMetricProvider metricProvider,
        IMapper mapper
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _metricProvider = metricProvider ?? throw new ArgumentNullException(nameof(metricProvider));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<StalkModel> ResetAsync(StalkModel stalkModel)
    {
        stalkModel.UserPlaylistResult = RequestStatus.Default;

        stalkModel.Playlists = new CategoryViewModel<PlaylistModel>();
        stalkModel.Artists = new CategoryViewModel<ArtistModel>();
        stalkModel.Genres = new CategoryViewModel<GenreModel>();
        stalkModel.Tracks = new CategoryViewModel<Track>();
        stalkModel.AudioFeatures = new CategoryViewModel<AudioFeaturesModel>();
        stalkModel.Metrics = new CategoryViewModel<Metric>();

        stalkModel.Processing = (false, default);

        var metrics = (await _metricProvider.GetAllAsync()).ToList();

        foreach (var metric in metrics)
            stalkModel.Metrics.TryAdd(metric.Id, metric);

        return stalkModel;
    }

    protected CategoryViewModel<T> GetCategoryViewModel<T>(StalkModel stalkModel) where T : ISpotifyStandardObject =>
        typeof(T).Name switch
        {
            nameof(PlaylistModel) => stalkModel.Playlists as CategoryViewModel<T>,
            nameof(ArtistModel) => stalkModel.Artists as CategoryViewModel<T>,
            nameof(GenreModel) => stalkModel.Genres as CategoryViewModel<T>,
            nameof(Track) => stalkModel.Tracks as CategoryViewModel<T>,
            nameof(AudioFeaturesModel) => stalkModel.AudioFeatures as CategoryViewModel<T>,
            nameof(Metric) => stalkModel.Metrics as CategoryViewModel<T>,
            _ => throw new NotSupportedException($"Type `{typeof(T).Name}` not supported")
        };

    public StalkModel IncrementCount<T>(StalkModel stalkModel) where T : ISpotifyStandardObject
    {
        var categoryViewModel = GetCategoryViewModel<T>(stalkModel);
        categoryViewModel.Processed++;
        return stalkModel;
    }

    protected StalkModel UpdateProcessing<T>(StalkModel stalkModel, bool processing) where T : ISpotifyStandardObject
    {
        var categoryViewModel = GetCategoryViewModel<T>(stalkModel);
        categoryViewModel.InProcess = processing;
        return stalkModel;
    }

    public List<T> GetItems<T>(StalkModel stalkModel) where T : ISpotifyStandardObject =>
        GetCategoryViewModel<T>(stalkModel).GetItems();

    public StalkModel RegisterPlaylists(StalkModel stalkModel, IEnumerable<Playlist> playlists)
    {
        if (!playlists?.Any() ?? false) return stalkModel;

        foreach (var playlist in
            playlists
            // not every list is owned by the username (don't know why), so filter it here
            .Where(x => x.Owner.Id.Equals(stalkModel.UserName, StringComparison.OrdinalIgnoreCase)).ToList())

            stalkModel.Playlists.TryAdd(playlist.Id, _mapper.Map<PlaylistModel>(playlist));

        return stalkModel;
    }

    public StalkModel RegisterTrack(
        StalkModel stalkModel,
        PlaylistModel playlist,
        Track track
    )
    {
        // if a track has a null / empty ID, we can't use it for anything.
        if (track == null || string.IsNullOrEmpty(track?.Id))
        {
            _logger.LogWarning("Track is null or has a null ID. Skipping");
            return stalkModel;
        }

        playlist.Tracks.TryAdd(track.Id, track);

        if (!stalkModel.Tracks.TryAdd(track.Id, track))
            return stalkModel; // if track wasn't added now, it was already added, so don't need to do anything more.

        foreach (var artist in track.Artists)
        {
            if (string.IsNullOrEmpty(artist.Id))
            {
                _logger.LogWarning("Artist has a null ID. Skipping");
                continue;
            };

            if (stalkModel.Artists.TryAdd(artist.Id, _mapper.Map<ArtistModel>(artist)))
            {
                // if artist was just added now, instantiate their track list
                stalkModel.Artists.Items[artist.Id].Tracks = new ConcurrentDictionary<string, Track>();
            };

            stalkModel.Artists.Items[artist.Id].Tracks.TryAdd(track.Id, track);
        }
        return stalkModel;
    }

    public StalkModel RegisterGenre(StalkModel stalkModel, ArtistModel artist)
    {
        foreach (var genre in artist.Genres)
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

            foreach (var track in artist.Tracks)
                stalkModel.Genres.Items[genre].Tracks.TryAdd(track.Key, track.Value);
        }
        return stalkModel;
    }

    public StalkModel RegisterAudioFeature(StalkModel stalkModel, AudioFeaturesModel audioFeatures)
    {
        if (audioFeatures == null) return stalkModel;
        stalkModel.AudioFeatures.TryAdd(audioFeatures.Id, audioFeatures);
        return CalculateAllMetrics(stalkModel, audioFeatures);
    }

    protected StalkModel CalculateAllMetrics(StalkModel stalkModel, AudioFeaturesModel currentAudioFeaturesModel)
    {
        foreach (var metric in stalkModel.Metrics.Items)
        {
            var newMetric = CalculateMetric(stalkModel, metric.Value, currentAudioFeaturesModel);

            stalkModel.Metrics.Items.TryUpdate(
                metric.Key,
                newMetric,
                metric.Value);
        }
        return stalkModel;
    }

    protected Metric CalculateMetric(
        StalkModel stalkModel,
        Metric metric,
        AudioFeaturesModel currentAudioFeaturesModel // sending this in so we can tell if it's the new min/max
        )
    {
        var values = stalkModel.AudioFeatures.Items
            .Select(x => x.Value)
            .Select(metric.Field)
            .Where(x => x.HasValue)
            .Select(x => x.Value);

        var min = values.Min(); // compare to losers instead
        metric.Average = values.Average();
        var max = values.Max();

        var thisValue = metric.Field(currentAudioFeaturesModel);

        if (thisValue.HasValue)
        {
            if (!metric.Loser.HasValue || thisValue > metric.Winner.Value.MetricValue)
                metric.Winner = (thisValue.Value, stalkModel.Tracks.Items[currentAudioFeaturesModel.Id]);

            if (!metric.Loser.HasValue || thisValue < metric.Loser.Value.MetricValue)
                metric.Loser = (thisValue.Value, stalkModel.Tracks.Items[currentAudioFeaturesModel.Id]);
        }

        // calculate marker position for average
        var mp = metric.Average / (metric.Max - metric.Min);
        if (mp < 0)
            mp += 1.0;
        metric.MarkerPosition = _plotAreaWidth * mp;

        return (metric);
    }
}
