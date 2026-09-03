using SportsTracker.App.Cache;
using SportsTracker.App.Common;
using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN;
using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Integrations.ESPN.Mappers;
using SportsTracker.App.Models.Rankings;

namespace SportsTracker.App.Services
{
    public interface IRankingsService
    {
        Task<LeagueRankings?> GetRankingsAsync(League league, CancellationToken cancellationToken = default);
    }

    public sealed class RankingsService(IEspnApiClient espnApiClient, ICacheService cache, ILogger<RankingsService> logger) : EspnCachedServiceBase(espnApiClient, cache), IRankingsService
    {
        public async Task<LeagueRankings?> GetRankingsAsync(League league, CancellationToken cancellationToken = default)
        {
            if (league != League.CFB)
            {
                logger.LogWarning("Rankings Not Supported for {League}", league);

                return null;
            }

            int season = DateTime.UtcNow.Year;
            
            LeagueRankings? cached = await cache.GetAsync<LeagueRankings>(CacheKeys.Rankings(league, season));

            if (cached is not null)
            {
                return cached;
            }
            
            logger.LogInformation("Fetching {League} Rankings for {Season}", league, season);

            string endpoint = EspnEndpoints.Rankings(league);

            ApiResult<RankingsResponseDto> result = await espnApiClient.GetAsync<RankingsResponseDto>(endpoint, cancellationToken);

            if (!result.Success || result.Value is null)
            {
                logger.LogWarning("Failed to Fetch {League} Rankings for {Season}", league, season);
                
                return null;
            }

            LeagueRankings rankings = RankingsMapper.Map(result.Value, season);

            await cache.SetAsync(CacheKeys.Rankings(league, season), rankings, TimeSpan.FromMinutes(30));
            
            return rankings;
        }
    }
}