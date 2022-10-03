namespace SpotifyStalker.Service;

public class PlaylistQueryService : IPlaylistQueryService
{
    private readonly IApiQueryService _apiQueryService;

    private readonly IStalkModelTransformer _stalkModelTransformer;

    private readonly IOptions<SpotifyApiSettings> _options;

    public PlaylistQueryService(
        IApiQueryService apiQueryService,
        IStalkModelTransformer stalkModelTransformer,
        IOptions<SpotifyApiSettings> options
        )
    {
        _apiQueryService = apiQueryService ?? throw new ArgumentNullException(nameof(apiQueryService));
        _stalkModelTransformer = stalkModelTransformer ?? throw new ArgumentNullException(nameof(stalkModelTransformer));
        _options = options ?? throw new ArgumentNullException(nameof(options));
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
            var playlistResult = await _apiQueryService.QueryAsync<PlaylistModel>(playlist.Value.Id, _options.Value.Limits.PlaylistTrack);

            incrementCountCallback();

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
    }

}
