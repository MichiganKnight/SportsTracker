using Microsoft.Extensions.Caching.Memory;

namespace SportsTracker.Backend.Cache
{
    public sealed class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        
        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetOrCreateAsync<T>(string key, TimeSpan expiration, Func<Task<T>> factory)
        {
            return await _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = expiration;

                return await factory();
            });
        }
        
        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}