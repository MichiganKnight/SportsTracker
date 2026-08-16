using System.Text.Json.Serialization;

namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Sport
{
    public sealed class FootballSituationDto
    {
        public int? Down { get; init; }
        public int? Distance { get; init; }

        public int? YardLine { get; init; }
        public int? YardsToEndzone { get; init; }

        public string? DownDistanceText { get; init; }
        public string? ShortDownDistanceText { get; init; }

        public string? PossessionText { get; init; }

        [JsonPropertyName("possession")]
        public string? PossessionTeamId { get; init; }
        
        public bool? IsRedZone { get; init; }
    }
}