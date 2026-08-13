using SportsTracker.Backend.Integrations.ESPN.DTOs.BoxScore;
using SportsTracker.Backend.Integrations.ESPN.Mappers;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.PlayByPlay;

namespace SportsTracker.Backend.Services.Implementations
{
    public sealed class PlayByPlayService : IPlayByPlayService
    {
        private readonly IGameSummaryService _gameSummaryService;
        private readonly ILogger<PlayByPlayService> _logger;

        public PlayByPlayService(IGameSummaryService gameSummaryService, ILogger<PlayByPlayService> logger)
        {
            _gameSummaryService = gameSummaryService;
            _logger = logger;
        }

        public async Task<GamePlayByPlay?> GetPlayByPlayAsync(League league, string gameId, CancellationToken cancellationToken = default)
        {
            GameSummaryResponseDto? summary = await _gameSummaryService.GetGameSummaryAsync(league, gameId, cancellationToken);

            if (summary?.Plays is null)
            {
                _logger.LogWarning("Game Summary Contained no Play-by-Play for {League} {GameId}", league, gameId);
                
                return null;
            }
            
            return PlayByPlayMapper.Map(summary, gameId, league);
        }
    }
}