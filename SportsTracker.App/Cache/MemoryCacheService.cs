using Microsoft.Extensions.Caching.Memory;

namespace SportsTracker.App.Cache
{
    public sealed class MemoryCacheService(IMemoryCache cache) : ICacheService
    {
        public Task<T?> GetAsync<T>(string key)
        {
            cache.TryGetValue(key, out T? value);

            return Task.FromResult(value);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            cache.Set(key, value, expiration);
            
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            cache.Remove(key);
            
            return Task.CompletedTask;
        }
    }
}