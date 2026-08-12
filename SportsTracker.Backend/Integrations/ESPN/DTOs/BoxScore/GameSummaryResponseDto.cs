using SportsTracker.Backend.Integrations.ESPN.DTOs.PlayByPlay;

namespace SportsTracker.Backend.Integrations.ESPN.DTOs.BoxScore
{
    public sealed class GameSummaryResponseDto
    {
        public BoxScoreDto? Boxscore { get; init; }
        
        public List<PlayDto>? Plays { get; init; }
    }
}