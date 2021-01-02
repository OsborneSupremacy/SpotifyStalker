using System.Threading.Tasks;

namespace SpotifyStalker.Interface
{
    public interface IApiRequestService
    {
        Task<T> GetAsync<T>(string url);
    }
}
