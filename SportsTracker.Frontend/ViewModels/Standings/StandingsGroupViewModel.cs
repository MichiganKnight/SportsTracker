namespace SportsTracker.Frontend.ViewModels.Standings
{
    public sealed class StandingsGroupViewModel
    {
        public string Name { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public IReadOnlyList<TeamStandingViewModel> Teams { get; init; } = [];
    }
}