using Spotify.Interface;
using System.Threading.Tasks;

namespace SpotifyStalker.Interface
{
    public interface IApiQueryService
    {
        Task<T> QueryAsync<T>(string userName) where T : IApiRequestObject, new();

        Task<T> QueryAsync<T>(string id, int limit) where T : IApiRequestObject, new();
    }
}
