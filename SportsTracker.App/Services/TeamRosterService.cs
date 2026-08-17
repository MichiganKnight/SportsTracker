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
    public interface ITeamRosterService
    {
        Task<TeamRoster?> GetTeamRosterAsync(League league, string teamId, CancellationToken cancellationToken = default);
    }
    
    public sealed class TeamRosterService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<TeamRosterService> logger) : EspnCachedServiceBase(espnApiClient, cache), ITeamRosterService
    {
        private readonly CacheOptions _cacheOptions = cacheOptions.Value;

        public Task<TeamRoster?> GetTeamRosterAsync(League league, string teamId, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<TeamRosterResponseDto, TeamRoster>(league, $"Team Roster for {teamId}", CacheKeys.TeamRoster(league, teamId), EspnEndpoints.TeamRoster(league, teamId), dto => TeamRosterMapper.Map(dto, league, teamId),
                _ => TimeSpan.FromMinutes(_cacheOptions.TeamRosterMinutes), logger, cancellationToken);
        }
    }
}