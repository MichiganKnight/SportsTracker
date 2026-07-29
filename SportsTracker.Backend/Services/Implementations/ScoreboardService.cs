using SportsTracker.Backend.Integrations.ESPN;
using SportsTracker.Backend.Integrations.ESPN.DTOs.Scoreboard;
using SportsTracker.Backend.Integrations.ESPN.Endpoints;
using SportsTracker.Backend.Integrations.ESPN.Mappers;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;

namespace SportsTracker.Backend.Services.Implementations
{
    public class ScoreboardService : IScoreboardService
    {
        private readonly IEspnApiClient _espnApiClient;
        
        public ScoreboardService(IEspnApiClient espnApiClient)
        {
            _espnApiClient = espnApiClient;
        }
        
        public async Task<IReadOnlyList<Game>> GetScoreboardAsync(League league, CancellationToken cancellationToken = default)
        {
            string endpoint = EspnEndpoints.Scoreboard(league);
            
            ScoreboardResponseDto? response = await _espnApiClient.GetAsync<ScoreboardResponseDto>(endpoint, cancellationToken);

            if (response is null)
            {
                return [];
            }

            return ScoreboardMapper.ToGames(response, league).ToList();
        }
    }
}