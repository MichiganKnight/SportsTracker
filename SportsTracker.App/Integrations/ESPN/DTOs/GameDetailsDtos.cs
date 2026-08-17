namespace SportsTracker.App.Integrations.ESPN.DTOs
{
    public sealed class BroadcastDto
    {
        public string? Market { get; init; }
        
        public List<string>? Names { get; init; } = [];
    }
    
    public sealed class FeaturedAthleteDto
    {
        public string? Name { get; init; }
        public string? DisplayName { get; init; }
        public string? ShortDisplayName { get; init; }
        public string? Abbreviation { get; init; }
        
        public long? PlayerId { get; init; }
        
        public AthleteDto? Athlete { get; init; }
        public TeamReferenceDto? Team { get; init; }
        
        public List<GameDetailsStatDto>? Statistics { get; init; } = [];
    }
    
    public sealed class GameDetailsCompetitionDto
    {
        public string? Id { get; init; }
        
        public DateTime? Date { get; init; }
        
        public int? Attendance { get; init; }
        
        public bool? NeutralSite { get; init; }
        public bool? PlayByPlayAvailable { get; init; }
        
        public VenueDto? Venue { get; init; }
        
        public List<GameDetailsCompetitorDto>? Competitors { get; init; } = [];
        
        public StatusDto? Status { get; init; }
        
        public List<BroadcastDto>? Broadcasts { get; init; } = [];
        
        public string? Broadcast { get; init; }
        
        public List<HeadlineDto>? Headlines { get; init; } = [];
    }
    
    public sealed class GameDetailsCompetitorDto
    {
        public string? Id { get; init; }
        
        public string? HomeAway { get; init; }
        
        public bool? Winner { get; init; }
        
        public GameDetailsTeamDto? Team { get; init; }
        
        public string? Score { get; init; }
        
        public List<LineScoreDto>? LineScores { get; init; } = [];
        public List<GameDetailsStatDto>? Statistics { get; init; } = [];
        public List<ProbablePitcherDto>? Probables { get; init; } = [];
        
        public int? Hits { get; init; }
        public int? Errors { get; init; }
        
        public List<RecordDto>? Records { get; init; }
    }

    public sealed class GameDetailsResponseDto
    {
        public string? Id { get; init; }
        public string? Uid { get; init; }
        
        public DateTime? Date { get; init; }
        
        public string? Name { get; init; }
        public string? ShortName { get; init; }
        
        public List<GameDetailsCompetitionDto>? Competitions { get; init; } = [];
        
        public StatusDto? Status { get; init; }
    }
    
    public sealed class GameDetailsStatDto
    {
        public string? Name { get; init; }
        public string? Abbreviation { get; init; }
        public string? DisplayValue { get; init; }
        public string? RankDisplayValue { get; init; }
    }
    
    public sealed class GameDetailsTeamDto
    {
        public string? Id { get; init; }
        
        public string? Location { get; init; }
        public string? Name { get; init; }
        public string? Abbreviation { get; init; }
        public string? DisplayName { get; init; }
        public string? ShortDisplayName { get; init; }
        public string? Color { get; init; }
        public string? AlternateColor { get; init; }
        public string? Logo { get; init; }
    }
    
    public sealed class HeadlineDto
    {
        public string? Type { get; init; }
        public string? Description { get; init; }
        public string? ShortLinkText { get; init; }
    }
    
    public sealed class LineScoreDto
    {
        public double? Value { get; init; }
        
        public string? DisplayValue { get; init; }
        
        public int? Period { get; init; }
    }
    
    public sealed class ProbablePitcherDto
    {
        public string? Name { get; init; }
        public string? DisplayName { get; init; }
        public string? ShortDisplayName { get; init; }
        public string? Abbreviation { get; init; }
        
        public long? PlayerId { get; init; }
        
        public AthleteDto? Athlete { get; init; }
        
        public List<GameDetailsStatDto>? Statistics { get; init; } = [];
        
        public string? Record { get; init; }
    }
}