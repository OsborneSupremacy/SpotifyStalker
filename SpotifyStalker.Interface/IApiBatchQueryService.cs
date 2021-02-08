using Spotify.Interface;
using SpotifyStalker.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyStalker.Interface
{
    public interface IApiBatchQueryService<T> where T : IApiBatchRequestObject, new()
    {
        void AddToQueue(string id);

        bool QueueIsEmpty();

        Task<(int CountOfItemsQueried, RequestStatus RequestStatus, T ResultCollection)> QueryAsync();
    }
}
