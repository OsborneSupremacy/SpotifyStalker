using System.Net.Http;
using System.Threading.Tasks;

namespace SpotifyStalker.Interface;

public interface IAuthorizedHttpClientFactory
{
    Task<HttpClient> CreateClientAsync();
}
