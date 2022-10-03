namespace SpotifyStalker.Interface;

public interface IArtistQueryService
{
    public Task QueryAsync(
            StalkModel viewModel,
            Action<string> statusUpdateCallback,
            Action<int> incrementCountCallback
            );
}
