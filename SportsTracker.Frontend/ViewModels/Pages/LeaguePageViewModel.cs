using SportsTracker.Frontend.ViewModels.Dashboard;
using SportsTracker.Shared.Enums;

namespace SportsTracker.Frontend.ViewModels.Pages
{
    public sealed class LeaguePageViewModel
    {
        public League League { get; init; }
        
        public string LeagueName { get; init; } = string.Empty;
        public string Icon { get; init; } = string.Empty;
        public DateTime LastUpdatedUtc { get; init; }
        
        
        public IReadOnlyList<GameCardViewModel> LiveGames { get; init; } = [];
        public IReadOnlyList<GameCardViewModel> UpcomingGames { get; init; } = [];
        public IReadOnlyList<GameCardViewModel> FinalGames { get; init; } = [];
        
        public int LiveCount => LiveGames.Count;
        public int UpcomingCount => UpcomingGames.Count;
        public int FinalCount => FinalGames.Count;
        
        public int TotalGames => LiveCount + UpcomingCount + FinalCount;
        
        public bool HasLiveGames => LiveCount > 0;
        public bool HasUpcomingGames => UpcomingCount > 0;
        public bool HasFinalGames => FinalCount > 0;
    }
}