namespace SpotifyStalker.Interface;

public interface IAudioFeaturesQueryService
{
    public Task QueryAsync(
        StalkModel viewModel,
        Action<string> statusUpdateCallback,
        Action<int> incrementCountCallback
        );
}
