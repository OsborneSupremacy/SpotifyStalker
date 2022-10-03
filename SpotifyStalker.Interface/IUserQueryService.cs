namespace SpotifyStalker.Interface;

public interface IUserQueryService
{
    public Task QueryUserAsync(
            StalkModel viewModel,
            Action stateHasChangedCallback
            );
}
