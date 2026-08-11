namespace SportsTracker.Shared.Models.GameDetails
{
    public sealed class LineScore
    {
        public int Period { get; init; }
        
        public double Value { get; init; }
        
        public string DisplayValue { get; init; } = string.Empty;
    }
}