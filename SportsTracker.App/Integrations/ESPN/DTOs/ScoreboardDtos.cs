using System.Text.Json;

namespace SportsTracker.App.Integrations.ESPN.DTOs
{
    public class ScoreboardResponseDto
    {
        public List<ScoreboardLeagueDto> Leagues { get; init; } = [];
        
        public List<EventDto> Events { get; init; } = [];
    }
    
    public class ScoreboardLeagueDto
    {
        public string Id { get; init; } = string.Empty;
        
        public string Name { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public List<EspnLogoDto> Logos { get; init; } = [];
    }
    
    public sealed class EventDto
    {
        public string Id { get; init; } = string.Empty;
        
        public DateTime Date { get; init; }
        
        public bool NeutralSite { get; init; }

        public List<CompetitionDto> Competitions { get; init; } = [];
    }
    
    public sealed class CompetitionDto
    {
        public List<CompetitorDto> Competitors { get; init; } = [];

        public StatusDto Status { get; init; } = new();
        
        public JsonElement? Situation { get; init; }
        
        public VenueDto? Venue { get; init; }
    }

    public sealed class CompetitorDto
    {
        public string HomeAway { get; init; } = string.Empty;
        public string Score { get; init; } = "0";

        public TeamDto Team { get; init; } = new();
        
        public List<RecordDto> Records { get; init; } = [];
    }
}