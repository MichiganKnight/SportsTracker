using SportsTracker.App.Cache;
using SportsTracker.App.Common;
using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN;

namespace SportsTracker.App.Services
{
    public abstract class EspnCachedServiceBase(IEspnApiClient espnApiClient, ICacheService cache)
    {
        protected async Task<TModel?> GetOrFetchAsync<TDto, TModel>(League league, string resourceName, string cacheKey, string endpoint, Func<TDto, TModel?> mapper, Func<TModel, TimeSpan> cacheLifetimeFactory, ILogger logger, CancellationToken cancellationToken) where TDto : class where TModel : class
        {
            TModel? cached = await cache.GetAsync<TModel>(cacheKey);

            if (cached is not null)
            {
                return cached;
            }
            
            logger.LogInformation("Fetching {League} {Resource}", league, resourceName);

            ApiResult<TDto> result = await espnApiClient.GetAsync<TDto>(endpoint, cancellationToken);

            if (!result.Success || result.Value is null)
            {
                logger.LogWarning("Unable to Fetch {League} {Resource}: {Message}", league, resourceName, result.Error?.Message);
                
                return null;
            }
            
            TModel? model = mapper(result.Value);

            if (model is null)
            {
                logger.LogWarning("Unable to Map {League} {Resource}", league, resourceName);
                
                return null;
            }
            
            TimeSpan cacheLifetime = cacheLifetimeFactory(model);
            
            await cache.SetAsync(cacheKey, model, cacheLifetime);
            
            return model;
        }
    }
}