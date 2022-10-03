namespace SpotifyStalker.Interface;

public interface IUserQueryService
{
    public Task QueryAsync(
            StalkModel viewModel,
            Action stateHasChangedCallback
            );
}
