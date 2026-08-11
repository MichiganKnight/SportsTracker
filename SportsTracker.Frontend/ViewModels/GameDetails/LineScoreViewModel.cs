namespace SportsTracker.Frontend.ViewModels.GameDetails
{
    public sealed class LineScoreViewModel
    {
        public int Period { get; init; }
        
        public string DisplayValue { get; init; } = string.Empty;
    }
}