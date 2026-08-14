using SportsTracker.Backend.Integrations.ESPN.DTOs.BoxScore;
using SportsTracker.Backend.Integrations.ESPN.DTOs.PlayByPlay;

namespace SportsTracker.Backend.Integrations.ESPN.DTOs.GameSummary
{
    public sealed class GameSummaryResponseDto
    {
        public BoxScoreDto? Boxscore { get; init; }
        
        public List<PlayDto>? Plays { get; init; }
        
        public DrivesDto? Drives { get; init; }
        
        public GameSummaryMetaDto? Meta { get; init; }
    }
}