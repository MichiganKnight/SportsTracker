using SportsTracker.Shared.Enums;

namespace SportsTracker.Frontend.ViewModels.Standings
{
    public sealed class StandingsGroupViewModel
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public StandingsGroupType Type { get; init; }
        
        public IReadOnlyList<TeamStandingViewModel> Teams { get; init; } = [];
    }
}