namespace SportsTracker.App.Integrations.ESPN.DTOs.Athlete
{
    public sealed class AthleteGameLogResponseDto
    {
        public List<string> Labels { get; init; } = [];
        public List<string> Names { get; init; } = [];
        public List<string> DisplayNames { get; init; } = [];
        
        public Dictionary<string, AthleteGameLogEventDto> Events { get; init; } = new();
        
        public List<AthleteGameLogSeasonTypeDto> SeasonTypes { get; init; } = [];
        public List<AthleteGameLogGlossaryDto> Glossary { get; init; } = [];
    }

    public sealed class AthleteGameLogSeasonTypeDto
    {
        public string? DisplayName { get; init; }
        public string? DisplayTeam { get; init; }
        
        public List<AthleteGameLogCategoryDto> Categories { get; init; } = [];
        
        public AthleteGameLogSummaryDto? Summary { get; init; }
    }

    public sealed class AthleteGameLogCategoryDto
    {
        public string? DisplayName { get; init; }
        public string? Type { get; init; }
        public string? SplitType { get; init; }
        
        public List<AthleteGameLogEntryDto> Events { get; init; } = [];
        
        public List<string> Totals { get; init; } = [];
    }

    public sealed class AthleteGameLogEntryDto
    {
        public string? EventId { get; init; }
        
        public List<string> Stats { get; init; } = [];
    }

    public sealed class AthleteGameLogEventDto
    {
        public string? Id { get; init; }
        
        public DateTimeOffset? GameDate { get; init; }
        
        public string? GameResult { get; init; }
        public string? Score { get; init; }
        public string? AtVs { get; init; }
        public string? EventNote { get; init; }
        public string? HomeTeamId { get; init; }
        public string? AwayTeamId { get; init; }
        public string? HomeTeamScore { get; init; }
        public string? AwayTeamScore { get; init; }
        
        public AthleteGameLogTeamDto? Team { get; init; }
        public AthleteGameLogTeamDto? Opponent { get; init; }
    }

    public sealed class AthleteGameLogTeamDto
    {
        public string? Id { get; init; }
        
        public string? Abbreviation { get; init; }
        public string? DisplayName { get; init; }
        public string? Logo { get; init; }
    }

    public sealed class AthleteGameLogSummaryDto
    {
        public string? DisplayName { get; init; }
        
        public List<AthleteGameLogSummaryStatDto> Stats { get; init; } = [];
    }
    
    public sealed class AthleteGameLogSummaryStatDto
    {
        public string? DisplayName { get; init; }
        public string? Type { get; init; }
        
        public List<string> Stats { get; init; } = [];
    }

    public sealed class AthleteGameLogGlossaryDto
    {
        public string? Abbreviation { get; init; }
        public string? DisplayName { get; init; }
    }
}