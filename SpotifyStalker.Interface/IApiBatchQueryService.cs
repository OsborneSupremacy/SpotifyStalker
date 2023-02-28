namespace SpotifyStalker.Interface;

public interface IApiBatchQueryService<T> where T : IApiBatchRequestObject, new()
{
    void AddToQueue(string id);

    bool QueueIsEmpty();

    Task<(Outcome<T>, int)> QueryAsync();
}
