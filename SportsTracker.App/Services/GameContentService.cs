using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Integrations.ESPN.Mappers;
using SportsTracker.App.Models.BoxScore;
using SportsTracker.App.Models.PlayByPlay;

namespace SportsTracker.App.Services
{
    public interface IGameContentService
    {
        Task<GameBoxScore?> GetBoxScoreAsync(League league, string gameId, CancellationToken cancellationToken = default);
        
        Task<GamePlayByPlay?> GetPlayByPlayAsync(League league, string gameId, CancellationToken cancellationToken = default);
    }
    
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