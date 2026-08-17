using SportsTracker.App.Enums;
using SportsTracker.App.ViewModels.GameInfo;
using SportsTracker.App.ViewModels.Golf;

namespace SportsTracker.App.ViewModels.LeagueInfo
{
    public sealed class LeaguePageViewModel
    {
        public League League { get; init; }
        
        public string LeagueName { get; init; } = string.Empty;
        public string Icon { get; init; } = string.Empty;
        
        public DateTime LastUpdatedUtc { get; init; }
        
        public IReadOnlyList<GolfEventCardViewModel> GolfEvents { get; init; } = [];

        public GameSectionViewModel Live { get; init; } = new();
        public GameSectionViewModel Upcoming { get; init; } = new();
        public GameSectionViewModel Final { get; init; } = new();

        public bool IsGolf => League == League.PGA;
        
        public int LiveCount => Live.Games.Count;
        public int UpcomingCount => Upcoming.Games.Count;
        public int FinalCount => Final.Games.Count;
        
        public int TotalGames => LiveCount + UpcomingCount + FinalCount;
        
        public bool HasLiveGames => LiveCount > 0;
        public bool HasUpcomingGames => UpcomingCount > 0;
        public bool HasFinalGames => FinalCount > 0;
        
        public bool HasGolfEvents => GolfEvents.Count > 0;
        
        public int TotalEvents => IsGolf ? GolfEvents.Count : TotalGames;
    }
}