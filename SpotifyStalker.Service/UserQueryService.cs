﻿namespace SpotifyStalker.Service;

public class UserQueryService : IUserQueryService
{
    private readonly IStalkModelTransformer _stalkModelTransformer;

    private readonly IUserPlaylistsQueryService _userPlaylistsQueryService;

    private readonly IArtistQueryService _artistQueryService;

    private readonly IPlaylistQueryService _playlistsQueryService;

    private readonly IApiBatchQueryService<AudioFeaturesModelCollection> _audioFeaturesQueryService;

    public UserQueryService(
        IStalkModelTransformer stalkModelTransformer,
        IUserPlaylistsQueryService userPlaylistsQueryService,
        IArtistQueryService artistQueryService,
        IPlaylistQueryService playlistsQueryService,
        IApiBatchQueryService<AudioFeaturesModelCollection> audioFeaturesQueryService
        )
    {
        _stalkModelTransformer = stalkModelTransformer ?? throw new ArgumentNullException(nameof(stalkModelTransformer));
        _userPlaylistsQueryService = userPlaylistsQueryService ?? throw new ArgumentNullException(nameof(userPlaylistsQueryService));
        _artistQueryService = artistQueryService ?? throw new ArgumentNullException(nameof(artistQueryService));
        _playlistsQueryService = playlistsQueryService ?? throw new ArgumentNullException(nameof(playlistsQueryService));
        _audioFeaturesQueryService = audioFeaturesQueryService ?? throw new ArgumentNullException(nameof(audioFeaturesQueryService));
    }

    public async Task QueryAsync(
        StalkModel viewModel,
        Action stateHasChangedCallback
        )
    {
        viewModel = await _stalkModelTransformer.ResetAsync(viewModel);

        if (!await _userPlaylistsQueryService
            .QueryAsync(viewModel, setProcessingMessage, stateHasChangedCallback))
        {
            clearProcessingMessage();
            return;
        }

        await _playlistsQueryService
            .QueryAsync
            (
                viewModel,
                setProcessingMessage,
                () => {
                    incrementCount<PlaylistModel>();
                }
            );

        await _artistQueryService
            .QueryAsync
            (
                viewModel,
                setProcessingMessage,
                incrementBy => {
                    incrementCount<ArtistModel>(incrementBy);
                }
            );

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