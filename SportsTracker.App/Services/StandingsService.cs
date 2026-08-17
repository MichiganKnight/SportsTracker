using Microsoft.Extensions.Options;
using SportsTracker.App.Cache;
using SportsTracker.App.Config;
using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN;
using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Integrations.ESPN.Mappers;
using SportsTracker.App.Models.Standings;

namespace SportsTracker.App.Services
{
    public interface IStandingsService
    {
        Task<LeagueStandings?> GetStandingsAsync(League league, CancellationToken cancellationToken = default);
    }
    
    public sealed class StandingsService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<StandingsService> logger) : EspnCachedServiceBase(espnApiClient, cache), IStandingsService
    {
        private readonly CacheOptions _cacheOptions = cacheOptions.Value;

        public Task<LeagueStandings?> GetStandingsAsync(League league, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<StandingsResponseDto, LeagueStandings>(league, "Standings", CacheKeys.Standings(league), EspnEndpoints.Standings(league), dto => StandingsMapper.Map(dto, league),
                _ => TimeSpan.FromMinutes(_cacheOptions.StandingsMinutes), logger, cancellationToken);
        }
    }
}