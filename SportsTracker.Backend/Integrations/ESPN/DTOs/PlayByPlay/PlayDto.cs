namespace SportsTracker.Backend.Integrations.ESPN.DTOs.PlayByPlay
{
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
}