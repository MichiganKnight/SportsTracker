namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Team
{
    public sealed class TeamRosterGroupDto
    {
        public string? Position { get; init; }
        
        public List<RosterAthleteDto> Items { get; init; } = [];
    }
}