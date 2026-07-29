namespace SportsTracker.Backend.Cache
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        
        Task SetAsync<T>(string key, T value, TimeSpan expiration);
        
        Task<T?> GetOrCreateAsync<T>(string key, TimeSpan expiration, Func<Task<T>> factory);
        
        void Remove(string key);
    }
}