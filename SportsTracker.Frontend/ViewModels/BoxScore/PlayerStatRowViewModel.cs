namespace SportsTracker.Frontend.ViewModels.BoxScore
{
    public sealed class PlayerStatRowViewModel
    {
        public string AthleteId { get; init; } = string.Empty;
        
        public string Name { get; init; } = string.Empty;
        public string ShortName { get; init; } = string.Empty;
        
        public string? Headshot { get; init; }
        public string? Position { get; init; }
        
        public bool Starter { get; init; }
        public bool IsSubstitute => !Starter;
        
        public int? BatOrder { get; init; }
        
        public string? Note { get; init; }
        
        public IReadOnlyList<string> Stats { get; init; } = [];
    }
}