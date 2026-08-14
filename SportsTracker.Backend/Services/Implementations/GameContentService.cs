using SportsTracker.Backend.Integrations.ESPN.DTOs.GameSummary;
using SportsTracker.Backend.Integrations.ESPN.Mappers;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.BoxScore;
using SportsTracker.Shared.Models.PlayByPlay;

namespace SportsTracker.Backend.Services.Implementations
{
    public sealed class GameContentService(IGameSummaryService gameSummaryService) : IGameContentService
    {
        public async Task<GameBoxScore?> GetBoxScoreAsync(League league, string gameId, CancellationToken cancellationToken = default)
        {
            GameSummaryResponseDto? summary = await gameSummaryService.GetGameSummaryAsync(league, gameId, cancellationToken);

            if (summary?.Boxscore is null)
            {
                return null;
            }

            return BoxScoreMapper.Map(summary.Boxscore, gameId, league);
        }

        public async Task<GamePlayByPlay?> GetPlayByPlayAsync(League league, string gameId, CancellationToken cancellationToken = default)
        {
            GameSummaryResponseDto? summary = await gameSummaryService.GetGameSummaryAsync(league, gameId, cancellationToken);

            if (summary is null)
            {
                return null;
            }

            return PlayByPlayMapper.Map(summary, gameId, league);
        }
    }
}