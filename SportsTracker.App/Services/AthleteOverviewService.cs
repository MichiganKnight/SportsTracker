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
    public interface IAthleteOverviewService
    {
        Task<AthleteOverview?> GetAthleteOverviewAsync(League league, string athleteId, CancellationToken cancellationToken = default);
    }
    
    public sealed class AthleteOverviewService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<AthleteOverviewService> logger) : EspnCachedServiceBase(espnApiClient, cache), IAthleteOverviewService
    {
        private readonly CacheOptions _cacheOptions = cacheOptions.Value;
        
        public Task<AthleteOverview?> GetAthleteOverviewAsync(League league, string athleteId, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<AthleteOverviewResponseDto, AthleteOverview>(league, $"Athlete Overview for {athleteId}", CacheKeys.AthleteOverview(league, athleteId), EspnEndpoints.AthleteOverview(league, athleteId),
                AthleteOverviewMapper.Map, _ => TimeSpan.FromMinutes(_cacheOptions.AthleteOverviewMinutes), logger, cancellationToken);
        }
    }
}