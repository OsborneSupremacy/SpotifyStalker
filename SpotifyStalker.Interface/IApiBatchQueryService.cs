using System.Threading.Tasks;
using LanguageExt.Common;
using Spotify.Interface;

namespace SpotifyStalker.Interface;

public interface IApiBatchQueryService<T> where T : IApiBatchRequestObject, new()
{
    void AddToQueue(string id);

    bool QueueIsEmpty();

    Task<(Result<T>, int)> QueryAsync();
}
