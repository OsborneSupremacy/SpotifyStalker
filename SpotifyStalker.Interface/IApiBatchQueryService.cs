using System.Threading.Tasks;
using Spotify.Interface;
using SpotifyStalker.Model;

namespace SpotifyStalker.Interface;

public interface IApiBatchQueryService<T> where T : IApiBatchRequestObject, new()
{
    void AddToQueue(string id);

    bool QueueIsEmpty();

    Task<(int CountOfItemsQueried, RequestStatus RequestStatus, T ResultCollection)> QueryAsync();
}
