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
    public interface IAthleteDetailsService
    {
        Task<AthleteDetails?> GetAthleteDetailsAsync(League league, string athleteId, CancellationToken cancellationToken = default);
    }
    
    public sealed class AthleteDetailsService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<AthleteDetailsService> logger) : EspnCachedServiceBase(espnApiClient, cache), IAthleteDetailsService
    {
        private readonly CacheOptions _cacheOptions = cacheOptions.Value;
        
        public async Task<AthleteDetails?> GetAthleteDetailsAsync(League league, string athleteId, CancellationToken cancellationToken = default)
        {
            return await GetOrFetchAsync<AthleteProfileResponseDto, AthleteDetails>(league, $"Athlete Details for {athleteId}", CacheKeys.AthleteDetails(league, athleteId), EspnEndpoints.AthleteDetails(league, athleteId), dto => AthleteDetailsMapper.Map(dto, league),
                _ => TimeSpan.FromMinutes(_cacheOptions.AthleteMinutes), logger, cancellationToken);
        }
    }
}