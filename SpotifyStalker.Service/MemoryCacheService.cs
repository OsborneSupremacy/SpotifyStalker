using Microsoft.Extensions.Caching.Memory;
using SpotifyStalker.Interface;
using System;
using System.Threading.Tasks;

namespace SpotifyStalker.Service
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<T> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> getDataFunction)
        {
            if (_cache.TryGetValue(cacheKey, out var cachedObject))
                return (T)cachedObject;

            var data = await getDataFunction.Invoke();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetPriority(CacheItemPriority.NeverRemove);

            _cache.Set(cacheKey, data, cacheEntryOptions);
            return data;
        }
    }
}
