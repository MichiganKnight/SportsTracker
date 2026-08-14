using SportsTracker.Backend.Integrations.ESPN.DTOs.Team;

namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Scoreboard
{
    public class ScoreboardLeagueDto
    {
        public string Id { get; init; } = string.Empty;
        
        public string Name { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public List<TeamLogoDto> Logos { get; init; } = [];
    }
}