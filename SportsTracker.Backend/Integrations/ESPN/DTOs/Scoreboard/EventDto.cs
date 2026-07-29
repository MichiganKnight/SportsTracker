namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Scoreboard
{
    public sealed class EventDto
    {
        public string Id { get; init; } = string.Empty;
        
        public DateTime Date { get; init; }
        
        public bool NeutralSite { get; init; }

        public List<CompetitionDto> Competitions { get; init; } = [];
    }
}