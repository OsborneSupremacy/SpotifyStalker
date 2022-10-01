using System.Net.Http;

namespace SpotifyStalker.Interface;

public interface IAuthorizedHttpClientFactory
{
    Task<HttpClient> CreateClientAsync();
}
