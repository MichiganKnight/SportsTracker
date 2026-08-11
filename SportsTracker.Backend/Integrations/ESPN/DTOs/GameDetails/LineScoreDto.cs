namespace SportsTracker.Backend.Integrations.ESPN.DTOs.GameDetails
{
    public sealed class LineScoreDto
    {
        public double? Value { get; init; }
        
        public string? DisplayValue { get; init; }
        
        public int? Period { get; init; }
    }
}