
namespace SpotifyStalker.Service;

[ServiceLifetime(ServiceLifetime.Scoped)]
[RegistrationTarget(typeof(IUserPlaylistsQueryService))]
public class UserPlaylistsQueryService : IUserPlaylistsQueryService
{
    private readonly IApiQueryService _apiQueryService;

    private readonly IStalkModelTransformer _stalkModelTransformer;

    private readonly SpotifyApiSettings _settings;

    public UserPlaylistsQueryService(
        IApiQueryService apiQueryService,
        IStalkModelTransformer stalkModelTransformer,
        SpotifyApiSettings settings
        )
    {
        _apiQueryService = apiQueryService ?? throw new ArgumentNullException(nameof(apiQueryService));
        _stalkModelTransformer = stalkModelTransformer ?? throw new ArgumentNullException(nameof(stalkModelTransformer));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task<bool> QueryAsync(
        StalkModel viewModel,
        Action<string> statusUpdateCallback,
        Action stateHasChangedCallback
        )
    {
        statusUpdateCallback("Looking up playlists");

        var queryResult = await _apiQueryService
            .QueryAsync<UserPlaylistsModel>(viewModel.UserName, _settings.Limits.UserPlaylist);

        if(queryResult.IsFaulted)
        {
            viewModel.UserPlaylistResult = queryResult.Exception switch
            {
                RequestException x => x.RequestStatus,
                _ => RequestStatus.Failed
            };
            return false;
        }

        viewModel.UserPlaylistResult = RequestStatus.Success;
        statusUpdateCallback("Registering playlists");
        viewModel = _stalkModelTransformer.RegisterPlaylists(viewModel, queryResult.Value.Playlists);
        stateHasChangedCallback();
        return true;
    }
}
