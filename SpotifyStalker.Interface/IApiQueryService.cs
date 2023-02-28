namespace SpotifyStalker.Interface;

public interface IApiQueryService
{
    Task<Outcome<T>> QueryAsync<T>(string userName) where T : IApiRequestObject, new();

    Task<Outcome<T>> QueryAsync<T>(string id, int limit) where T : IApiRequestObject, new();

    Task<Outcome<T>> QueryAsync<T>(IEnumerable<string> ids) where T : IApiRequestObject, new();

    Task<Outcome<T>> QueryAsync<T>(string id, int limit, int offset) where T : IApiRequestObject, new();
}
