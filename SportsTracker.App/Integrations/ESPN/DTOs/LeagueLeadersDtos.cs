namespace SportsTracker.App.Integrations.ESPN.DTOs
{
    public sealed class LeagueLeadersResponseDto
    {
        public LeagueLeadersSeasonDto? Season { get; init; }
        public LeagueLeadersStatsDto? Stats { get; init; }
    }

    public sealed class LeagueLeadersSeasonDto
    {
        public int? Year { get; init; }
        
        public string? DisplayName { get; init; }
        
        public int? Type { get; init; }
        
        public string? Name { get; init; }
    }
    
    public sealed class LeagueLeadersStatsDto
    {
        public string? Id { get; init; }
        
        public string? Name { get; init; }
        public string? Abbreviation { get; init; }
        
        public List<LeagueLeaderCategoryDto>? Categories { get; init; } = [];
    }
    
    public sealed class LeagueLeaderCategoryDto
    {
        public string? Name { get; init; }
        public string? DisplayName { get; init; }
        public string? ShortDisplayName { get; init; }
        public string? Abbreviation { get; init; }
        
        public List<LeagueLeaderDto>? Leaders { get; init; } = [];
    }

    public sealed class LeagueLeaderDto
    {
        public string? DisplayValue { get; init; }
        public double? Value { get; init; }
        
        public LeagueLeaderStatisticsDto? Statistics { get; init; }
        public LeagueLeaderAthleteDto? Athlete { get; init; }
        public LeagueLeaderTeamDto? Team { get; init; }
    }

    public sealed class LeagueLeaderStatisticsDto
    {
        public LeagueLeaderSplitsDto? Splits { get; init; }
    }
    
    public sealed class LeagueLeaderSplitsDto
    {
        public List<LeagueLeaderStatCategoryDto>? Categories { get; init; } = [];
    }

    public sealed class LeagueLeaderStatCategoryDto
    {
        public string? Name { get; init; }
        
        public List<LeagueLeaderStatDto>? Stats { get; init; } = [];
    }

    public sealed class LeagueLeaderStatDto
    {
        public string? Name { get; init; }
        
        public string? DisplayValue { get; init; }
        public double? Value { get; init; }
    }

    public sealed class LeagueLeaderAthleteDto
    {
        public string? Id { get; init; }
        
        public string? DisplayName { get; init; }
        public string? ShortName { get; init; }
        public string? Jersey { get; init; }
        
        public LeagueLeaderHeadshotDto? Headshot { get; init; }
        public LeagueLeaderTeamDto? Team { get; init; }
    }
    
    public sealed class LeagueLeaderHeadshotDto
    {
        public string? Href { get; init; }
        public string? Alt { get; init; }
    }

    public sealed class LeagueLeaderTeamDto
    {
        public string? Id { get; init; }
        
        public string? Name { get; init; }
        public string? Abbreviation { get; init; }
        public string? DisplayName { get; init; }
        
        public List<EspnLogoDto>? Logos { get; init; } = [];
    }
}