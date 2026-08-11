namespace SportsTracker.Backend.Integrations.ESPN.DTOs.GameDetails
{
    public sealed class BroadcastDto
    {
        public string? Market { get; init; }
        
        public List<string>? Names { get; init; } = [];
    }
}