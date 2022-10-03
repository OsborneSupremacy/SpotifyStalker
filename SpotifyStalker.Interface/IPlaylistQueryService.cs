namespace SpotifyStalker.Interface;

public interface IPlaylistQueryService
{
    public Task QueryAsync(
            StalkModel viewModel,
            Action<string> statusUpdateCallback,
            Action incrementCountCallback
            );
}
