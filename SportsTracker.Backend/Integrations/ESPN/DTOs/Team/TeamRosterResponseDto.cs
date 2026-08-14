namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Team
{
    public sealed class TeamRosterResponseDto
    {
        public TeamRosterSeasonDto? Season { get; init; }
        
        public List<TeamRosterGroupDto> Athletes { get; init; } = [];
    }
}