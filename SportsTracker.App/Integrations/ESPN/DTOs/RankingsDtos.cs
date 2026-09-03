namespace SportsTracker.App.Integrations.ESPN.DTOs
{
    public sealed class RankingsResponseDto
    {
        public List<RankingPollDto>? Rankings { get; init; } = [];
    }

    public sealed class RankingPollDto
    {
        public string? Id { get; init; }
        
        public string? Name { get; init; }
        public string? ShortName { get; init; }
        public string? Type { get; init; }
        
        public RankingOccurrenceDto? Occurrence { get; init; }
        
        public DateTime? Date { get; init; }
        
        public RankingSeasonDto? Season { get; init; }
        
        public DateTime? LastUpdated { get; init; }
        
        public List<RankingEntryDto>? Ranks { get; init; } = [];
    }

    public sealed class RankingOccurrenceDto
    {
        public int? Number { get; init; }
        
        public string? Type { get; init; }
        public string? Value { get; init; }
        public string? DisplayValue { get; init; }
    }

    public sealed class RankingSeasonDto
    {
        public int? Year { get; init; }
        
        public string? DisplayValue { get; init; }
    }

    public sealed class RankingEntryDto
    {
        public int? Current { get; init; }
        public int? Previous { get; init; }
        public double? Points { get; init; }
        public int? FirstPlaceVotes { get; init; }
        
        public string? Trend { get; init; }
        
        public RankingTeamDto? Team { get; init; }
        
        public string? RecordSummary { get; init; }
    }

    public sealed class RankingTeamDto
    {
        public string? Id { get; init; }
        
        public string? Location { get; init; }
        public string? Name { get; init; }
        public string? Nickname { get; init; }
        public string? Abbreviation { get; init; }
        public string? Logo { get; init; }
        
        public RankingGroupDto? Groups { get; init; }
    }
    
    public sealed class RankingGroupDto
    {
        public string? Id { get; init; }
        
        public string? ShortName { get; init; }
    }
}