using SportsTracker.App.Enums;
using SportsTracker.App.ViewModels.GameInfo;

namespace SportsTracker.App.ViewModels.LeagueInfo
{
    public sealed class LeagueSectionViewModel
    {
        public League League { get; init; }
        
        public string LeagueName { get; init; } = string.Empty;
        public string Icon { get; init; } = string.Empty;

        public IReadOnlyList<GameCardViewModel> Games { get; init; } = [];
        
        public int GameCount => Games.Count;
        public bool HasGames => Games.Count > 0;
        
        public int LiveGames { get; init; }
        public int TotalGames { get; init; }

        public int DisplayedGames => Games.Count;

        public bool HasMoreGames => TotalGames > DisplayedGames;
        
        public DateTime LastUpdatedUtc { get; init; }
    }
}