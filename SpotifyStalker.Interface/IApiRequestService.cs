namespace SpotifyStalker.Interface;

public interface IApiRequestService
{
    Task<Outcome<T>> GetAsync<T>(string url);
}
