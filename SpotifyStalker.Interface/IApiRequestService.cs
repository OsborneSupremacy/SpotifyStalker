namespace SpotifyStalker.Interface;

public interface IApiRequestService
{
    Task<Result<T>> GetAsync<T>(string url);
}
