using System.Text.Json.Serialization;
using SportsTracker.Backend.Integrations.ESPN.DTOs.Baseball;
using SportsTracker.Backend.Integrations.ESPN.DTOs.Common;

namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Scoreboard
{
    public sealed class CompetitionDto
    {
        public List<CompetitorDto> Competitors { get; init; } = [];

        public StatusDto Status { get; init; } = new();
        
        [JsonPropertyName("situation")]
        public SituationDto? BaseballSituation { get; init; }
        public VenueDto? Venue { get; init; }
    }

    public sealed class CompetitorDto
    {
        public string HomeAway { get; init; } = string.Empty;
        public string Score { get; init; } = "0";

        public TeamDto Team { get; init; } = new();
        
        public List<RecordDto> Records { get; init; } = [];
    }
}