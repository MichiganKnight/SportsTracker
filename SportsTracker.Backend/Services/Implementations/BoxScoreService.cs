using SportsTracker.Backend.Integrations.ESPN.DTOs.BoxScore;
using SportsTracker.Backend.Integrations.ESPN.Mappers;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.BoxScore;

namespace SportsTracker.Backend.Services.Implementations
{
    public sealed class BoxScoreService : IBoxScoreService
    {
        private readonly IGameSummaryService _gameSummaryService;
        private readonly ILogger<BoxScoreService> _logger;

        public BoxScoreService(IGameSummaryService gameSummaryService, ILogger<BoxScoreService> logger)
        {
            _gameSummaryService = gameSummaryService;
            _logger = logger;
        }

        public async Task<GameBoxScore?> GetBoxScoreAsync(League league, string gameId, CancellationToken cancellationToken = default)
        {
            GameSummaryResponseDto? summary = await _gameSummaryService.GetGameSummaryAsync(league, gameId, cancellationToken);

            if (summary?.Boxscore is null)
            {
                _logger.LogWarning("Game Summary Contained no Boxscore for {League} {GameId}", league, gameId);
                
                return null;
            }
            
            return BoxScoreMapper.Map(summary.Boxscore, gameId, league);
        }
    }
}