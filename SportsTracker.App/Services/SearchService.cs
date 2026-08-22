using SportsTracker.App.Cache;
using SportsTracker.App.Common;
using SportsTracker.App.Integrations.ESPN;
using SportsTracker.App.Integrations.ESPN.DTOs.Search;
using SportsTracker.App.Integrations.ESPN.Mappers;
using SportsTracker.App.Models;

namespace SportsTracker.App.Services
{
    public interface ISearchService
    {
        Task<SearchResults?> SearchAsync(string query, CancellationToken cancellationToken = default);
    }
    
    public sealed class SearchService(IEspnApiClient espnApiClient, ICacheService cache, ILogger<SearchService> logger) : ISearchService
    {
        private static readonly TimeSpan CacheLifetime = TimeSpan.FromMinutes(5);
        
        public async Task<SearchResults?> SearchAsync(string query, CancellationToken cancellationToken = default)
        {
            string normalizedQuery = query.Trim();

            if (string.IsNullOrWhiteSpace(normalizedQuery))
            {
                return null;
            }

            string cacheKey = $"search:{normalizedQuery.ToLowerInvariant()}";
            
            SearchResults? cached = await cache.GetAsync<SearchResults>(cacheKey);

            if (cached is not null)
            {
                return cached;
            }
            
            logger.LogInformation("Searching ESPN for {Query}", normalizedQuery);
            
            ApiResult<EspnSearchResponseDto> result = await espnApiClient.GetAsync<EspnSearchResponseDto>(EspnEndpoints.Search(normalizedQuery), cancellationToken);

            if (!result.Success || result.Value is null)
            {
                logger.LogWarning("Unable to Search ESPN for {Query}: {Message}", normalizedQuery, result.Error?.Message);
                
                return null;
            }

            SearchResults searchResults = SearchMapper.Map(result.Value, normalizedQuery);
            
            await cache.SetAsync(cacheKey, searchResults, CacheLifetime);
            
            return searchResults;
        }
    }
}