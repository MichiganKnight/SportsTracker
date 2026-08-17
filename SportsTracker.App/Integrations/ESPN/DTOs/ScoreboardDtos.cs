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
        
        public string Name { get; init; } = string.Empty;
        
        public DateTime Date { get; init; }
        public DateTime? EndDate { get; init; }
        
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
        public string Id { get; init; }
        
        // Team Sports
        public string? HomeAway { get; init; }
        public TeamDto? Team { get; init; }
        
        // Golf
        public int? Order { get; init; }
        public GolfAthleteDto? Athlete { get; init; }
        
        // Shared
        public string? Score { get; init; }
        
        public List<RecordDto> Records { get; init; } = [];
        public List<GolfLineScoreDto> LineScores { get; init; } = [];
    }

    public sealed class GolfAthleteDto
    {
        public string? FullName { get; init; }
        public string? DisplayName { get; init; }
        public string? ShortName { get; init; }
        
        public GolfFlagDto? Flag { get; init; }
    }
    
    public sealed class GolfFlagDto
    {
        public string? Href { get; init; }
        public string? Alt { get; init; }
    }

    public sealed class GolfLineScoreDto
    {
        public double? Value { get; init; }
        
        public string? DisplayValue { get; init; }
        
        public int? Period { get; init; }
        
        public GolfScoreTypeDto? ScoreType { get; init; }
        
        public List<GolfLineScoreDto> LineScores { get; init; } = [];
    }
    
    public sealed class GolfScoreTypeDto
    {
        public string? DisplayValue { get; init; }
    }
}