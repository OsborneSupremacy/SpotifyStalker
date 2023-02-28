namespace SpotifyStalker.Service;

[ServiceLifetime(ServiceLifetime.Scoped)]
[RegistrationTarget(typeof(IPlaylistQueryService))]
public class PlaylistQueryService : IPlaylistQueryService
{
    private readonly IApiQueryService _apiQueryService;

    private readonly IStalkModelTransformer _stalkModelTransformer;

    private readonly SpotifyApiSettings _settings;

    public PlaylistQueryService(
        IApiQueryService apiQueryService,
        IStalkModelTransformer stalkModelTransformer,
        SpotifyApiSettings settings
        )
    {
        _apiQueryService = apiQueryService ?? throw new ArgumentNullException(nameof(apiQueryService));
        _stalkModelTransformer = stalkModelTransformer ?? throw new ArgumentNullException(nameof(stalkModelTransformer));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task QueryAsync(
        StalkModel viewModel,
        Action<string> statusUpdateCallback,
        Action incrementCountCallback
        )
    {
        statusUpdateCallback("Getting playlist tracks");

        // at this point we only have playlist names and IDs.
        // get the track list for each playlist
        foreach (var playlist in viewModel.Playlists.Items)
        {
            var playlistResult = await _apiQueryService.QueryAsync<PlaylistModel>(playlist.Value.Id, _settings.Limits.PlaylistTrack);

            incrementCountCallback();

            if (playlistResult.IsFaulted)
                return;

            // add the tracks from the playlist api query to the list of all tracks
            foreach (var playlistModelTrack in playlistResult.Value.Items)
                viewModel = _stalkModelTransformer.RegisterTrack(viewModel, playlist.Value, playlistModelTrack.Track);
        }
    }
}
