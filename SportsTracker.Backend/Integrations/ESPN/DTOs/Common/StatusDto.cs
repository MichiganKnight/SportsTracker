namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Common
{
    public sealed class StatusDto
    {
        public StatusTypeDto Type { get; init; } = new();
        
        public int Period { get; init; }
        
        public string DisplayClock { get; init; } = string.Empty;
    }

    public sealed class StatusTypeDto
    {
        public string Name { get; init; } = string.Empty;
        public string ShortDetail { get; init; } = string.Empty;
        public string Detail { get; init; } = string.Empty;
        public string State { get; init; } = string.Empty;
        
        public bool Completed { get; init; }
    }
}