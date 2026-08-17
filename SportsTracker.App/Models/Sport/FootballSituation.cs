using System.Text.Json.Serialization;

namespace SportsTracker.App.Models.Sport
{
    public sealed class FootballSituation
    {
        public int? Down { get; init; }
        public int? Distance { get; init; }

        public int? YardLine { get; init; }
        public int? YardsToEndzone { get; init; }

        public string? DownDistanceText { get; init; }
        public string? ShortDownDistanceText { get; init; }

        [JsonPropertyName("possession")]
        public string? PossessionTeamId { get; init; }
        
        public string? PossessionText { get; init; }

        public bool IsRedZone { get; init; }
    }
}