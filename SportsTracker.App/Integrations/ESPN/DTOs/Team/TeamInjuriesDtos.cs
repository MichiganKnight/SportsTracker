using System.Text.Json.Serialization;

namespace SportsTracker.App.Integrations.ESPN.DTOs.Team
{
    public sealed class TeamInjuriesResponseDto
    {
        public int? Count { get; init; }
        public int? PageIndex { get; init; }
        public int? PageSize { get; init; }
        public int? PageCount { get; init; }

        public List<TeamInjuryReferenceDto> Items { get; init; } = [];
    }

    public sealed class TeamInjuryReferenceDto
    {
        [JsonPropertyName("$ref")]
        public string? Ref { get; init; }
    }

    public sealed class TeamInjuryDto
    {
        public string? Id { get; init; }

        public string? LongComment { get; init; }
        public string? ShortComment { get; init; }
        public string? Status { get; init; }

        public DateTime? Date { get; init; }

        public TeamInjuryReferenceDto? Athlete { get; init; }
        public TeamInjuryReferenceDto? Team { get; init; }

        public InjuryStatusDto? Type { get; init; }
        public InjuryDetailsDto? Details { get; init; }
    }

    public sealed class InjuryStatusDto
    {
        public string? Id { get; init; }

        public string? Name { get; init; }
        public string? Description { get; init; }
        public string? Abbreviation { get; init; }
    }

    public sealed class InjuryDetailsDto
    {
        public string? Type { get; init; }
        public string? Location { get; init; }
        public string? Detail { get; init; }
        public string? Side { get; init; }

        public DateOnly? ReturnDate { get; init; }
    }
}