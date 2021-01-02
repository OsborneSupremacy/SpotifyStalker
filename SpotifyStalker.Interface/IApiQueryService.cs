using Spotify.Interface;
using SpotifyStalker.Model;
using System.Threading.Tasks;

namespace SpotifyStalker.Interface
{
    public interface IApiQueryService
    {
        Task<(RequestStatus RequestStatus, T)> QueryAsync<T>(string userName) where T : IApiRequestObject, new();

        Task<(RequestStatus RequestStatus, T)> QueryAsync<T>(string id, int limit) where T : IApiRequestObject, new();
    }
}
