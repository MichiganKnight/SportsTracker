namespace SportsTracker.Frontend.ViewModels.BoxScore
{
    public sealed class TeamBoxScoreViewModel
    {
        public string TeamId { get; init; } = string.Empty;
        
        public string TeamName { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public string? Logo { get; init; }
        
        public IReadOnlyList<PlayerStatTableViewModel> Tables { get; init; } = [];
    }
}