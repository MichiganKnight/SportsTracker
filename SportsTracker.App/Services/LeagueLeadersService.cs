using SportsTracker.App.Cache;
using SportsTracker.App.Common;
using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN;
using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Integrations.ESPN.Mappers;
using SportsTracker.App.Models;

namespace SportsTracker.App.Services
{
    public interface ILeagueLeadersService
    {
        Task<LeagueLeaders?> GetLeadersAsync(League league, CancellationToken cancellationToken = default);
    }

    public sealed class LeagueLeadersService(IEspnApiClient espnApiClient, ICacheService cache, ILogger<LeagueLeadersService> logger) : EspnCachedServiceBase(espnApiClient, cache), ILeagueLeadersService
    {
        private static readonly IReadOnlyList<LeagueLeaderRequest> MlbRequests =
        [
            // Batting
            new("batting", "avg"),
            new("batting", "homeRuns"),
            new("batting", "RBIs"),
            new("batting", "runs"),
            new("batting", "OPS"),
            new("batting", "onBasePct"),
            new("batting", "slugAvg"),
            new("batting", "stolenBases"),
            new("batting", "hits"),
            new("batting", "WARBR"),

            // Pitching
            new("pitching", "ERA", false),
            new("pitching", "wins"),
            new("pitching", "strikeouts"),
            new("pitching", "saves"),
            new("pitching", "WHIP", false),
            new("pitching", "qualityStarts"),
            new("pitching", "holds"),
            new("pitching", "WARBR")
        ];
        
        public async Task<LeagueLeaders?> GetLeadersAsync(League league, CancellationToken cancellationToken = default)
        {
            LeagueLeaders? cached = await cache.GetAsync<LeagueLeaders>(CacheKeys.LeagueLeaders(league));

            if (cached is not null)
            {
                return cached;
            }
            
            int season = DateTime.UtcNow.Year;

            IReadOnlyList<LeagueLeaderRequest> requests = GetRequests(league);

            if (requests.Count == 0)
            {
                logger.LogWarning("League Leaders Not Configured for {League}", league);
                
                return null;
            }
            
            logger.LogInformation("Fetching {League} League Leaders for {Season}", league, season);
            
            Task<LeaderCategory?>[] tasks = requests.Select(request => FetchCategoryAsync(league, season, request, cancellationToken)).ToArray();
            
            LeaderCategory?[] results = await Task.WhenAll(tasks);

            List<LeaderCategory> categories = results.Where(category => category is not null).Select(category => category!).ToList();

            if (categories.Count == 0)
            {
                return null;
            }

            LeagueLeaders leaders = new()
            {
                Season = season,
                SeasonName = season.ToString(),
                Categories = categories
            };
            
            await cache.SetAsync(CacheKeys.LeagueLeaders(league), leaders, TimeSpan.FromMinutes(30));
            
            return leaders;
        }

        private async Task<LeaderCategory?> FetchCategoryAsync(League league, int season, LeagueLeaderRequest request, CancellationToken cancellationToken)
        {
            string endpoint = EspnEndpoints.LeagueLeadersByAthlete(league, request.Category, request.Statistic, season, seasonType: 2, limit: 5, descending: request.Descending);
            
            ApiResult<LeagueLeadersResponseDto> result = await espnApiClient.GetAsync<LeagueLeadersResponseDto>(endpoint, cancellationToken);

            if (!result.Success || result.Value is null)
            {
                logger.LogWarning("Failed to Fetch {League} Leader Category {Category}.{Statistic}", league, request.Category, request.Statistic);
                
                return null;
            }
            
            return LeagueLeadersMapper.MapCategory(result.Value, request.Category, request.Statistic);
        }

        private static IReadOnlyList<LeagueLeaderRequest> GetRequests(League league)
        {
            return league switch
            {
                League.MLB => MlbRequests,

                _ => []
            };
        }

        private sealed record LeagueLeaderRequest(string Category, string Statistic, bool Descending = true);
    }
}