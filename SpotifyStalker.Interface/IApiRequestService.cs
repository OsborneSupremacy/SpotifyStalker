using System.Threading.Tasks;
using SpotifyStalker.Model;

namespace SpotifyStalker.Interface;

public interface IApiRequestService
{
    Task<(RequestStatus RequestStatus, T Result)> GetAsync<T>(string url);
}
