using Microsoft.Extensions.Options;
using SportsTracker.App.Cache;
using SportsTracker.App.Config;
using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN;
using SportsTracker.App.Integrations.ESPN.DTOs.Athlete;
using SportsTracker.App.Integrations.ESPN.Mappers;
using SportsTracker.App.Models.AthleteInfo;

namespace SportsTracker.App.Services
{
    public interface IAthleteService
    {
        Task<AthleteDetails?> GetAthleteDetailsAsync(League league, string athleteId, CancellationToken cancellationToken = default);
        Task<AthleteOverview?> GetAthleteOverviewAsync(League league, string athleteId, CancellationToken cancellationToken = default);
        Task<AthleteStats?> GetAthleteStatsAsync(League league, string athleteId, CancellationToken cancellationToken = default);
    }
    
    public sealed class AthleteService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<AthleteService> logger) : EspnCachedServiceBase(espnApiClient, cache), IAthleteService
    {
        private readonly CacheOptions _cache = cacheOptions.Value;
        
        public Task<AthleteDetails?> GetAthleteDetailsAsync(League league, string athleteId, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<AthleteProfileResponseDto, AthleteDetails>(league, $"Athlete Details for {athleteId}", CacheKeys.AthleteDetails(league, athleteId), EspnEndpoints.AthleteDetails(league, athleteId),
                dto => AthleteDetailsMapper.Map(dto, league), _ => TimeSpan.FromMinutes(_cache.AthleteMinutes), logger, cancellationToken);
        }

        public Task<AthleteOverview?> GetAthleteOverviewAsync(League league, string athleteId, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<AthleteOverviewResponseDto, AthleteOverview>(league, $"Athlete Overview for {athleteId}", CacheKeys.AthleteOverview(league, athleteId), EspnEndpoints.AthleteOverview(league, athleteId),
                AthleteOverviewMapper.Map, _ => TimeSpan.FromMinutes(_cache.AthleteOverviewMinutes), logger, cancellationToken);
        }

        public Task<AthleteStats?> GetAthleteStatsAsync(League league, string athleteId, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<AthleteStatsResponseDto, AthleteStats>(league, $"Athlete Stats for {athleteId}", CacheKeys.AthleteStats(league, athleteId), EspnEndpoints.AthleteStats(league, athleteId), AthleteStatsMapper.Map,
                _ => TimeSpan.FromMinutes(_cache.AthleteStatsMinutes), logger, cancellationToken);
        }
    }
}