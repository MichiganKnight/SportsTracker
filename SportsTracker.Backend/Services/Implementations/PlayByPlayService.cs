using SportsTracker.Backend.Integrations.ESPN.DTOs.BoxScore;
using SportsTracker.Backend.Integrations.ESPN.Mappers;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.PlayByPlay;

namespace SportsTracker.Backend.Services.Implementations
{
    public sealed class PlayByPlayService(IGameSummaryService gameSummaryService, ILogger<PlayByPlayService> logger) : IPlayByPlayService
    {
        public async Task<GamePlayByPlay?> GetPlayByPlayAsync(League league, string gameId, CancellationToken cancellationToken = default)
        {
            GameSummaryResponseDto? summary = await gameSummaryService.GetGameSummaryAsync(league, gameId, cancellationToken);

            if (summary?.Plays is null)
            {
                logger.LogWarning("Game Summary Contained no Play-by-Play for {League} {GameId}", league, gameId);
                
                return null;
            }
            
            return PlayByPlayMapper.Map(summary, gameId, league);
        }
    }
}