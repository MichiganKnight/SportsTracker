namespace SportsTracker.App.Integrations.ESPN.DTOs
{
    public sealed class GameSummaryResponseDto
    {
        public BoxScoreDto? Boxscore { get; init; }
        
        public List<PlayDto>? Plays { get; init; }
        
        public DrivesDto? Drives { get; init; }
        
        public GameSummaryMetaDto? Meta { get; init; }
    }
    
    public sealed class GameSummaryMetaDto
    {
        public string? GameState { get; init; }
    }
    
    public sealed class DrivesDto
    {
        public List<DriveDto> Previous { get; init; }
        
        public DriveDto? Current { get; init; }
    }
    
    public sealed class DriveDto
    {
        public string? Id { get; init; }
        
        public string? Description { get; init; }
        public string? Result { get; init; }
        public string? ShortDisplayResult { get; init; }
        public string? DisplayResult { get; init; }
        
        public List<PlayDto>? Plays { get; init; } = [];
    }
    
    public sealed class PlayDto
    {
        public string? Id { get; init; }
        
        public string? SequenceNumber { get; init; }
        
        public PlayTypeDto? Type { get; init; }
        
        public string? Text { get; init; }
        
        public int? AwayScore { get; init; }
        public int? HomeScore { get; init; }
        
        public PlayPeriodDto? Period { get; init; }
        
        public bool? ScoringPlay { get; init; }
        
        public int? ScoreValue { get; init; }
        
        public PlayTeamReferenceDto? Team { get; init; }
        
        public PlayClockDto? Clock { get; init; }
        
        public string? AtBatId { get; init; }
        
        public string? SummaryType { get; init; }
        
        public int? Outs { get; init; }
        
        public PlaySituationDto? Start { get; init; }
        public PlaySituationDto? End { get; init; }
    }
    
    public sealed class PlayClockDto
    {
        public string? DisplayValue { get; init; }
    }
    
    public sealed class PlayPeriodDto
    {
        public string? Type { get; init; }
        
        public int? Number { get; init; }
        
        public string? DisplayValue { get; init; }
    }
    
    public sealed class PlaySituationDto
    {
        public int? Down { get; init; }
        public int? Distance { get; init; }
        
        public int? YardLine { get; init; }
        public int? YardsToEndzone { get; init; }

        public string? DownDistanceText { get; init; }
        public string? ShortDownDistanceText { get; init; }
        public string? PossessionText { get; init; }

        public PlayTeamDto? Team { get; init; }
    }

    public sealed class PlayTeamDto
    {
        public string? Id { get; init; }
    }
    
    public sealed class PlayTeamReferenceDto
    {
        public string? Id { get; init; }
    }
    
    public sealed class PlayTypeDto
    {
        public string? Id { get; init; }
        
        public string? Type { get; init; }
        public string? Text { get; init; }
        public string? Abbreviation { get; init; }
        public string? AlternativeText { get; init; }
    }
}