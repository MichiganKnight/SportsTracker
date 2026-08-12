namespace SportsTracker.Backend.Integrations.ESPN.DTOs.PlayByPlay
{
    public sealed class PlayPeriodDto
    {
        public string? Type { get; init; }
        
        public int? Number { get; init; }
        
        public string? DisplayValue { get; init; }
    }
}