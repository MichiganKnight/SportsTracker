using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using SportsTracker.Backend.Cache;
using SportsTracker.Backend.Config;
using SportsTracker.Backend.Hubs;
using SportsTracker.Backend.Integrations.ESPN;
using SportsTracker.Backend.Integrations.ESPN.DTOs.Scoreboard;
using SportsTracker.Backend.Integrations.ESPN.DTOs.Team;
using SportsTracker.Backend.Integrations.ESPN.Endpoints;
using SportsTracker.Backend.Integrations.ESPN.Mappers;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;
using SportsTracker.Shared.Models.GameInfo;

namespace SportsTracker.Backend.Services.Implementations
{
    public sealed class ScoreboardRefreshService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, IHubContext<ScoreboardHub> hub, IGameSummaryService gameSummaryService, ILogger<ScoreboardRefreshService> logger) : IScoreboardRefreshService
    {
        private readonly CacheOptions _cacheOptions = cacheOptions.Value;

        public async Task<TimeSpan?> RefreshAsync(League league, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Refreshing {League} Scoreboard...", league);

            string endpoint = EspnEndpoints.Scoreboard(league);
            
            ApiResult<ScoreboardResponseDto> result = await espnApiClient.GetAsync<ScoreboardResponseDto>(endpoint, cancellationToken);

            if (!result.Success || result.Value is null)
            {
                logger.LogWarning("Unable to Refresh {League}: {Message}", league, result.Error?.Message);
                
                return null;
            }
            
            CachedScoreboard? previousScoreboard = await cache.GetAsync<CachedScoreboard>(CacheKeys.Scoreboard(league));
            
            IReadOnlyList<Game> games = ScoreboardMapper.ToGames(result.Value, league).ToList();
            
            DateTime updatedUtc = DateTime.UtcNow;
            TimeSpan refreshInterval = GetRefreshInterval(games);
            TimeSpan cacheLifetime = GetCacheLifetime(refreshInterval);

            ScoreboardLeagueDto? leagueInfo = result.Value.Leagues.FirstOrDefault();
            
            string? leagueLogo = GetLeagueLogo(leagueInfo?.Logos, "default");
            string? leagueDarkLogo = GetLeagueLogo(leagueInfo?.Logos, "dark");

            await cache.SetAsync(CacheKeys.Scoreboard(league), new CachedScoreboard
            {
                League = league,
                Games = games,
                
                LeagueLogo = leagueLogo,
                LeagueDarkLogo = leagueDarkLogo,
                
                LastUpdatedUtc = updatedUtc
            }, cacheLifetime);

            await InvalidateGameSummariesAsync(league, games, previousScoreboard);
            
            await hub.Clients.All.SendAsync("ScoreboardUpdated", new ScoreboardUpdatedMessage
            {
                League = league.ToString(),
                UpdatedUtc = updatedUtc
            }, cancellationToken);
            
            logger.LogInformation("Cached {Count} Games for {League} | Next Refresh in {Interval}", games.Count, league, refreshInterval);
            
            return refreshInterval;
        }

        private async Task InvalidateGameSummariesAsync(League league, IReadOnlyList<Game> games, CachedScoreboard? previousScoreboard)
        {
            foreach (Game game in games)
            {
                Game? previousGame = previousScoreboard?.Games.FirstOrDefault(g => g.Id == game.Id);
                
                bool justFinished = game.IsFinal && previousGame is not null && !previousGame.IsFinal;

                if (!game.IsLive && !justFinished)
                {
                    continue;
                }
                
                await gameSummaryService.InvalidateAsync(league, game.Id);
            }
        }

        private TimeSpan GetRefreshInterval(IReadOnlyList<Game> games)
        {
            if (games.Any(game => game.IsLive))
            {
                return TimeSpan.FromSeconds(_cacheOptions.LiveScoreboardSeconds);
            }

            if (games.Any(game => game.IsUpcoming))
            {
                return TimeSpan.FromMinutes(_cacheOptions.ScheduledScoreboardMinutes);
            }

            if (games.Count > 0 && games.All(game => game.IsFinal))
            {
                return TimeSpan.FromMinutes(_cacheOptions.FinalScoreboardMinutes);
            }

            return TimeSpan.FromMinutes(_cacheOptions.ScheduledScoreboardMinutes);
        }

        private static TimeSpan GetCacheLifetime(TimeSpan refreshInterval)
        {
            return TimeSpan.FromTicks(refreshInterval.Ticks * 2);
        }

        private static string? GetLeagueLogo(IReadOnlyList<TeamLogoDto>? logos, string relation)
        {
            if (logos is null)
            {
                return null;
            }
            
            return logos.FirstOrDefault(logo => logo.Rel.Any(rel => rel.Equals(relation, StringComparison.OrdinalIgnoreCase)))?.Href;
        }
    }
}