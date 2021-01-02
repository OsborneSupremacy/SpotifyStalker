using SpotifyStalker.Model;
using System.Threading.Tasks;

namespace SpotifyStalker.Interface
{
    public interface IApiRequestService
    {
        Task<(RequestStatus RequestStatus, T)> GetAsync<T>(string url);
    }
}
