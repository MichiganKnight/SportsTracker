namespace SportsTracker.Backend.Integrations.ESPN.DTOs.BoxScore
{
    public sealed class BoxScoreAthleteEntryDto
    {
        public bool? Active { get; init; }
        
        public BoxScoreAthleteDto? Athlete { get; init; }
        
        public bool? Starter { get; init; }
        
        public int? BatOrder { get; init; }
        
        public BoxScorePositionDto? Position { get; init; }
        
        public List<BoxScoreNoteDto>? Notes { get; init; }
        
        public List<string>? Stats { get; init; } = [];
    }
}