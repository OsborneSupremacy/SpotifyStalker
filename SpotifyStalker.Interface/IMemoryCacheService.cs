using System;

namespace SpotifyStalker.Interface;

public interface IMemoryCacheService
{
    Task<T> GetOrCreateAsync<T>(
        string cacheKey,
        Func<Task<T>> getDataFunction
    );
}
