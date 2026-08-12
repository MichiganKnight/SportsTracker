namespace SportsTracker.Backend.Integrations.ESPN.DTOs.BoxScore
{
    public sealed class BoxScorePlayerTeamDto
    {
        public BoxScoreTeamDto? Team { get; init; }
        
        public List<BoxScoreStatTableDto>? Statistics { get; init; } = [];
        
        public int? DisplayOrder { get; init; }
    }
}