namespace SportsTracker.Frontend.ViewModels.Shared
{
    public sealed class TeamViewModel
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string ShortDisplayName { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public string? Logo { get; init; }
        public string? Record { get; init; }
        
        public int Score { get; init; }
        
        public string PrimaryColor { get; init; } = string.Empty;
        
        public string? AlternateColor { get; init; }
    }
}