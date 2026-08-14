using SportsTracker.Backend.Integrations.ESPN.DTOs.PlayByPlay;

namespace SportsTracker.Backend.Integrations.ESPN.DTOs.GameSummary
{
    public sealed class DriveDto
    {
        public string? Id { get; init; }
        
        public string? Description { get; init; }
        public string? Result { get; init; }
        public string? ShortDisplayResult { get; init; }
        public string? DisplayResult { get; init; }
        
        public List<PlayDto>? Plays { get; init; } = [];
    }
}