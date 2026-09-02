namespace SportsTracker.App.Integrations.ESPN.DTOs
{
    public sealed class LeagueLeadersResponseDto
    {
        public LeagueLeadersPaginationDto? Pagination { get; init; }
        
        public List<LeagueLeaderAthleteEntryDto>? Athletes { get; init; } = [];
        
        public LeagueLeadersSeasonDto? CurrentSeason { get; init; }
        public LeagueLeadersSeasonDto? RequestedSeason { get; init; }
        
        public List<LeagueLeaderCategoryMetadataDto>? Categories { get; init; } = [];
    }

    public sealed class LeagueLeadersPaginationDto
    {
        public int? Count { get; init; }
        public int? Limit { get; init; }
        public int? Page { get; init; }
        public int? Pages { get; init; }
    }

    public sealed class LeagueLeadersSeasonDto
    {
        public int? Year { get; init; }
        
        public string? DisplayName { get; init; }
        
        public LeagueLeadersSeasonTypeDto? Type { get; init; }
    }
    
    public sealed class LeagueLeadersSeasonTypeDto
    {
        public string? Id { get; init; }
        
        public int? Type { get; init; }
        
        public string? Name { get; init; }
    }
    
    public sealed class LeagueLeaderCategoryMetadataDto
    {
        public string? Name { get; init; }
        public string? DisplayName { get; init; }
        
        public List<string>? Labels { get; init; } = [];
        public List<string>? Names { get; init; } = [];
        public List<string>? DisplayNames { get; init; } = [];
    }

    public sealed class LeagueLeaderAthleteEntryDto
    {
        public LeagueLeaderAthleteDto? Athlete { get; init; }
        
        public List<LeagueLeaderAthleteCategoryDto>? Categories { get; init; } = [];
    }
    
    public sealed class LeagueLeaderAthleteDto
    {
        public string? Id { get; init; }
        
        public string? DisplayName { get; init; }
        public string? ShortName { get; init; }
        
        public LeagueLeaderHeadshotDto? Headshot { get; init; }
        
        public string? TeamId { get; init; }
        
        public string? TeamName { get; init; }
        public string? TeamShortName { get; init; }
        
        public List<EspnLogoDto>? TeamLogos { get; init; } = [];
    }
    
    public sealed class LeagueLeaderHeadshotDto
    {
        public string? Href { get; init; }
        public string? Alt { get; init; }
    }
    
    public sealed class LeagueLeaderAthleteCategoryDto
    {
        public string? Name { get; init; }
        public string? DisplayName { get; init; }
        
        public string? SplitId { get; init; }
        
        public List<string?>? Totals { get; init; } = [];
        
        public List<double?>? Values { get; init; } = [];
        
        public List<string?>? Ranks { get; init; } = [];
    }
}