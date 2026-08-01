using SportsTracker.Frontend.ViewModels.GameInfo;
using SportsTracker.Shared.Enums;

namespace SportsTracker.Frontend.ViewModels.LeagueInfo
{
    public sealed class LeaguePageViewModel
    {
        public League League { get; init; }
        
        public string LeagueName { get; init; } = string.Empty;
        public string Icon { get; init; } = string.Empty;
        public DateTime LastUpdatedUtc { get; init; }

        public GameSectionViewModel Live { get; init; } = new();
        public GameSectionViewModel Upcoming { get; init; } = new();
        public GameSectionViewModel Final { get; init; } = new();
        
        public int LiveCount => Live.Games.Count;
        public int UpcomingCount => Upcoming.Games.Count;
        public int FinalCount => Final.Games.Count;
        
        public int TotalGames => LiveCount + UpcomingCount + FinalCount;
        
        public bool HasLiveGames => LiveCount > 0;
        public bool HasUpcomingGames => UpcomingCount > 0;
        public bool HasFinalGames => FinalCount > 0;
    }
}