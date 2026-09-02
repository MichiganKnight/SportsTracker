using Microsoft.Extensions.Options;
using SportsTracker.App.Cache;
using SportsTracker.App.Config;
using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN;
using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Integrations.ESPN.Mappers;
using SportsTracker.App.Models;

namespace SportsTracker.App.Services
{
    public interface ILeagueLeaderService
    {
        Task<LeagueLeaders?> GetLeadersAsync(League league, CancellationToken cancellationToken = default);
    }

    public sealed class LeagueLeaderService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<LeagueLeaderService> logger) : EspnCachedServiceBase(espnApiClient, cache), ILeagueLeaderService
    {
        public Task<LeagueLeaders?> GetLeadersAsync(League league, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<LeagueLeadersResponseDto, LeagueLeaders>(league, "League Leaders", CacheKeys.LeagueLeaders(league), EspnEndpoints.LeagueStatistics(league), LeagueLeadersMapper.Map, _ => TimeSpan.FromMinutes(30),
                logger, cancellationToken);
        }
    }
}