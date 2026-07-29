namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Scoreboard
{
    public sealed class StatusDto
    {
        public StatusTypeDto Type { get; init; } = new();
    }

    public sealed class StatusTypeDto
    {
        public string Name { get; init; } = string.Empty;
        public string ShortDetail { get; init; } = string.Empty;
    }
}