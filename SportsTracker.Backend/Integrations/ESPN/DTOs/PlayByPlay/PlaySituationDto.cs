using SportsTracker.Backend.Integrations.ESPN.DTOs.Common;

namespace SportsTracker.Backend.Integrations.ESPN.DTOs.PlayByPlay
{
    public sealed class PlaySituationDto
    {
        public int? Down { get; init; }
        public int? Distance { get; init; }
        
        public int? YardLine { get; init; }
        public int? YardsToEndzone { get; init; }

        public string? DownDistanceText { get; init; }
        public string? ShortDownDistanceText { get; init; }
        public string? PossessionText { get; init; }

        public TeamDto? Team { get; init; }
    }
}