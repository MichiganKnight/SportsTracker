namespace SportsTracker.App.Integrations.ESPN.DTOs.Athlete
{
    public sealed class AthleteOverviewResponseDto
    {
        public AthleteOverviewStatisticsDto? Statistics { get; init; }
        
        public List<AthleteNewsDto> News { get; init; } = [];
        
        public AthleteRotowireDto? Rotowire { get; init; }
        
        public List<AthleteAwardDto> Awards { get; init; } = [];
        
        public AthleteFantasyDto? Fantasy { get; init; }
        
        public List<GolfRecentTournamentGroupDto> RecentTournaments { get; init; } = [];
        
        public GolfSeasonRankingsDto? SeasonRankings { get; init; }
    }

    public sealed class AthleteOverviewStatisticsDto
    {
        public string? DisplayName { get; init; }
        
        public List<string> Labels { get; init; } = [];
        public List<string> Names { get; init; } = [];
        public List<string> DisplayNames { get; init; } = [];
        
        public List<AthleteOverviewStatSplitDto> Splits { get; init; } = [];
    }
    
    public sealed class AthleteOverviewStatSplitDto
    {
        public string? DisplayName { get; init; }
        
        public List<string> Stats { get; init; } = [];
    }

    public sealed class AthleteNewsDto
    {
        public string? Headline { get; init; }
        public string? Description { get; init; }
        
        public DateTime? LastModified { get; init; }
        
        public List<AthleteNewsImageDto> Images { get; init; } = [];
    }

    public sealed class AthleteNewsImageDto
    {
        public string? Url { get; init; }
        public string? Name { get; init; }
        
        public int? Width { get; init; }
        public int? Height { get; init; }
    }

    public sealed class AthleteRotowireDto
    {
        public string? Headline { get; init; }
        public string? Story { get; init; }
        public string? Description { get; init; }
        public string? Published { get; init; }
    }

    public sealed class AthleteAwardDto
    {
        public string? Name { get; init; }
        public string? DisplayCount { get; init; }
        
        public List<string> Seasons { get; init; } = [];
    }

    public sealed class AthleteFantasyDto
    {
        public string? DraftRank { get; init; }
        public string? PositionRank { get; init; }
        public string? PercentOwned { get; init; }
        public string? Projection { get; init; }
    }
    
    public sealed class GolfSeasonRankingsDto
    {
        public string? DisplayName { get; init; }
        
        public List<GolfSeasonRankingDto> Categories { get; init; } = [];
    }
    
    public sealed class GolfSeasonRankingDto
    {
        public string? Name { get; init; }
        public string? DisplayName { get; init; }
        public string? ShortDisplayName { get; init; }
        public string? Abbreviation { get; init; }
        
        public double? Value { get; init; }
        public string? DisplayValue { get; init; }
        
        public int? Rank { get; init; }
        public string? RankDisplayValue { get; init; }
    }

    public sealed class GolfRecentTournamentGroupDto
    {
        public string? DisplayName { get; init; }
        
        public List<GolfRecentTournamentEventDto> EventStats { get; init; } = [];
    }

    public sealed class GolfRecentTournamentEventDto
    {
        public string? Id { get; init; }
        
        public DateTime? Date { get; init; }
        public DateTime? EndDate { get; init; }
        
        public string? Name { get; init; }
        
        public List<GolfRecentTournamentCompetitionDto> Competitions { get; init; } = [];
    }

    public sealed class GolfRecentTournamentCompetitionDto
    {
        public List<GolfRecentTournamentCompetitorDto> Competitors { get; init; } = [];
    }

    public sealed class GolfRecentTournamentCompetitorDto
    {
        public GolfTournamentScoreDto? Score { get; init; }
        public GolfRecentTournamentLineScoresDto? LineScores { get; init; }
        public GolfRecentTournamentStatusDto? Status { get; init; }
        
        public List<GolfSeasonRankingDto> Stats { get; init; } = [];
    }
    
    public sealed class GolfTournamentScoreDto
    {
        public double? Value { get; init; }
        public string? DisplayValue { get; init; }
    }

    public sealed class GolfRecentTournamentLineScoresDto
    {
        public List<GolfRecentTournamentLineScoreDTo> Items { get; init; } = [];
    }

    public sealed class GolfRecentTournamentLineScoreDTo
    {
        public double? Value { get; init; }
    }

    public sealed class GolfRecentTournamentStatusDto
    {
        public GolfRecentTournamentPositionDto? Position { get; init; }
    }

    public sealed class GolfRecentTournamentPositionDto
    {
        public string? DisplayName { get; init; }
    }
}