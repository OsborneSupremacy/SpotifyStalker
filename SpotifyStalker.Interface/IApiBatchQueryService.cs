namespace SpotifyStalker.Interface;

public interface IApiBatchQueryService<T> where T : IApiBatchRequestObject, new()
{
    void AddToQueue(string id);

    bool QueueIsEmpty();

    Task<(Result<T>, int)> QueryAsync();
}
