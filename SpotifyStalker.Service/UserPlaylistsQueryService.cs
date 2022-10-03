
namespace SpotifyStalker.Service;

public class UserPlaylistsQueryService : IUserPlaylistsQueryService
{
    private readonly IApiQueryService _apiQueryService;

    private readonly IStalkModelTransformer _stalkModelTransformer;

    private readonly IOptions<SpotifyApiSettings> _options;

    public UserPlaylistsQueryService(
        IApiQueryService apiQueryService,
        IStalkModelTransformer stalkModelTransformer,
        IOptions<SpotifyApiSettings> options
        )
    {
        _apiQueryService = apiQueryService ?? throw new ArgumentNullException(nameof(apiQueryService));
        _stalkModelTransformer = stalkModelTransformer ?? throw new ArgumentNullException(nameof(stalkModelTransformer));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<bool> QueryAsync(
        StalkModel viewModel,
        Action<string> statusUpdateCallback,
        Action stateHasChangedCallback
        )
    {
        statusUpdateCallback("Looking up playlists");

        var queryResult = await _apiQueryService
            .QueryAsync<UserPlaylistsModel>(viewModel.UserName, _options.Value.Limits.UserPlaylist);

        return queryResult.Match
        (
            model =>
            {
                viewModel.UserPlaylistResult = RequestStatus.Success;
                statusUpdateCallback("Registering playlists");
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
    }
}
