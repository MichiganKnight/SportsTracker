namespace SportsTracker.Shared.Models
{
    public sealed class GameSituation
    {
        public string Primary { get; init; } = string.Empty;
        public string? Secondary { get; init; }
        public string? Detail { get; init; }

        public bool HasDetail => !string.IsNullOrWhiteSpace(Primary) || !string.IsNullOrWhiteSpace(Secondary) || !string.IsNullOrWhiteSpace(Detail);
    }
}