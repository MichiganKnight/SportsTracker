namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Scoreboard
{
    public class ScoreboardResponseDto
    {
        public List<ScoreboardLeagueDto> Leagues { get; init; } = [];
        
        public List<EventDto> Events { get; init; } = [];
    }
}