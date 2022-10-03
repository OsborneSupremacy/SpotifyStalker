namespace SpotifyStalker.Interface;

public interface IUserPlaylistsQueryService
{
    public Task<bool> QueryAsync(
        StalkModel viewModel,
        Action<string> statusUpdateCallback,
        Action stateHasChangedCallback
        );
}
