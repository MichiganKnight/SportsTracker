namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Standings
{
    public sealed class StandingsEntryDto
    {
        public StandingTeamDto? Team { get; init; }
        
        public List<StandingStatDto>? Stats { get; init; } = [];
    }
}