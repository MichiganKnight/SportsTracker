using Microsoft.Extensions.Options;
using SportsTracker.App.Cache;
using SportsTracker.App.Config;
using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN;
using SportsTracker.App.Integrations.ESPN.DTOs.Team;
using SportsTracker.App.Integrations.ESPN.Mappers;
using SportsTracker.App.Models.TeamInfo;

namespace SportsTracker.App.Services
{
    public interface ITeamDetailsService
    {
        Task<TeamDetails?> GetTeamDetailsAsync(League league, string teamId, CancellationToken cancellationToken = default);
    }
    
    public sealed class TeamDetailsService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<TeamDetailsService> logger) : EspnCachedServiceBase(espnApiClient, cache), ITeamDetailsService
    {
        private readonly CacheOptions _cacheOptions = cacheOptions.Value;

        public Task<TeamDetails?> GetTeamDetailsAsync(League league, string teamId, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<TeamDetailsResponseDto, TeamDetails>(league, $"Team Details for {teamId}", CacheKeys.TeamDetails(league, teamId), EspnEndpoints.TeamDetails(league, teamId), dto => TeamDetailsMapper.Map(dto, league),
                _ => TimeSpan.FromMinutes(_cacheOptions.TeamMinutes), logger, cancellationToken);
        }
    }
}