namespace SportsTracker.Frontend.ViewModels.TeamInfo
{
    public sealed class TeamVenueViewModel
    {
        public string Id { get; init; } = string.Empty;

        public string Name { get; init; } = string.Empty;

        public string? City { get; init; }
        public string? State { get; init; }
        public string? ZipCode { get; init; }

        public bool? Grass { get; init; }
        public bool? Indoor { get; init; }

        public string? Image { get; init; }

        public string LocationDisplay => string.Join(", ", new[] { City, State }.Where(value => !string.IsNullOrWhiteSpace(value)));
    }
}