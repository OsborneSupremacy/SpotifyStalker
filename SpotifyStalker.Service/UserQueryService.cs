namespace SpotifyStalker.Service;

public class UserQueryService : IUserQueryService
{
    private readonly IStalkModelTransformer _stalkModelTransformer;

    private readonly IApiQueryService _apiQueryService;

    private readonly IApiBatchQueryService<ArtistModelCollection> _artistBatchQueryService;

    private readonly IApiBatchQueryService<AudioFeaturesModelCollection> _audioFeaturesQueryService;

    private readonly IOptions<SpotifyApiSettings> Options;

    public UserQueryService(
        IStalkModelTransformer stalkModelTransformer,
        IApiQueryService apiQueryService,
        IApiBatchQueryService<ArtistModelCollection> artistBatchQueryService,
        IApiBatchQueryService<AudioFeaturesModelCollection> audioFeaturesQueryService,
        IOptions<SpotifyApiSettings> options
        )
    {
        _stalkModelTransformer = stalkModelTransformer ?? throw new ArgumentNullException(nameof(stalkModelTransformer));
        _apiQueryService = apiQueryService ?? throw new ArgumentNullException(nameof(apiQueryService));
        _artistBatchQueryService = artistBatchQueryService ?? throw new ArgumentNullException(nameof(artistBatchQueryService));
        _audioFeaturesQueryService = audioFeaturesQueryService ?? throw new ArgumentNullException(nameof(audioFeaturesQueryService));
        Options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task QueryUserAsync(
        StalkModel viewModel,
        Action stateHasChangedCallback
        )
    {
        viewModel = await _stalkModelTransformer.ResetAsync(viewModel);

        setProcessingMessage("Looking up playlists");

        var userPlayListResult = await _apiQueryService.QueryAsync<UserPlaylistsModel>(viewModel.UserName, Options.Value.Limits.UserPlaylist);

        userPlayListResult.Match
        (
            model =>
            {
                viewModel.UserPlaylistResult = RequestStatus.Success;
                setProcessingMessage("Registering playlists");
                viewModel = _stalkModelTransformer.RegisterPlaylists(viewModel, model.Playlists);
                stateHasChangedCallback();
                return true;
            },
            exception =>
            {
                viewModel.UserPlaylistResult = exception switch
                {
                    RequestException x => x.RequestStatus,
                    _ => RequestStatus.Failed
                };
                return false;
            }
        );

        if (!viewModel.Playlists.Display)
        {
            clearProcessingMessage();
            return;
        }

        setProcessingMessage("Getting playlist tracks");

        // at this point we only have playlist names and IDs.
        // get the track list for each playlist
        foreach (var playlist in viewModel.Playlists.Items)
        {
            var playlistResult = await _apiQueryService.QueryAsync<PlaylistModel>(playlist.Value.Id, Options.Value.Limits.PlaylistTrack);

            incrementCount<PlaylistModel>();

            playlistResult.Match
            (
                model =>
                {
                    // add the tracks from the playlist api query to the list of all tracks
                    foreach (var playlistModelTrack in model.Items)
                        viewModel = _stalkModelTransformer.RegisterTrack(viewModel, playlist.Value, playlistModelTrack.Track);
                    return true;
                },
                exception =>
                {
                    return false;
                }
            );
        }

        setProcessingMessage("Looking up artists");

        // add all artists to batch query service
        foreach (var artist in viewModel.Artists.Items)
            _artistBatchQueryService.AddToQueue(artist.Key);

        while (!_artistBatchQueryService.QueueIsEmpty())
        {
            var (artistResult, artistItemsQueried) = await _artistBatchQueryService.QueryAsync();

            artistResult.Match
            (
                model =>
                {
                    // loop through results, assigning artist genres to artists
                    foreach (var result in model.Artists)
                    {
                        incrementCount<ArtistModel>();

                        var artist = viewModel.Artists.Items[result.Id];

                        artist.Genres = result.Genres;
                        _stalkModelTransformer.RegisterGenre(viewModel, artist);
                    }
                    return true;
                },
                exception =>
                {
                    incrementCount<ArtistModel>(artistItemsQueried);
                    return false;
                }
            );
        }

        setProcessingMessage("Looking up audio features");

        foreach (var track in viewModel.Tracks.Items)
        {
            if (!string.IsNullOrEmpty(track.Value.Id))
                _audioFeaturesQueryService.AddToQueue(track.Value.Id);
        }

        while (!_audioFeaturesQueryService.QueueIsEmpty())
        {
            var (audioFeaturesResult, audioFeaturesQueriedCount) = await _audioFeaturesQueryService.QueryAsync();

            audioFeaturesResult.Match
            (
                model =>
                {
                    foreach (var result in model.AudioFeaturesList)
                    {
                        incrementCount<Track>();
                        viewModel = _stalkModelTransformer.RegisterAudioFeature(viewModel, result);
                    }
                    return true;
                },
                exception =>
                {
                    incrementCount<Track>(audioFeaturesQueriedCount);
                    return false;
                }
            );
            // loop through results
        }

        clearProcessingMessage();

        void setProcessingMessage(string message) => viewModel.Processing = new ProcessingStage(true, message);

        void clearProcessingMessage() => viewModel.Processing = new ProcessingStage(false, default);

        void incrementCount<T>(int incrementBy = 1) where T : ISpotifyStandardObject
        {
            for (int x = 0; x < incrementBy; x++)
                viewModel = _stalkModelTransformer.IncrementCount<T>(viewModel);
            stateHasChangedCallback();
        }
    }
}
