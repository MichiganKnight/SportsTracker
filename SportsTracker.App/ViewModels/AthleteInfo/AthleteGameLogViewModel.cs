using SportsTracker.App.Enums;

namespace SportsTracker.App.ViewModels.AthleteInfo
{
    public sealed class AthleteGameLogViewModel
    {
        public League League { get; init; }
        
        public IReadOnlyList<AthleteGameLogColumnViewModel> Columns { get; init; } = [];
        public IReadOnlyList<AthleteGameLogSeasonViewModel> Seasons { get; init; } = [];

        public bool HasGames => Seasons.Any(season => season.Categories.Any(category => category.Games.Count > 0));
    }

    public sealed class AthleteGameLogColumnViewModel
    {
        public string Label { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
    }

    public sealed class AthleteGameLogSeasonViewModel
    {
        public string DisplayName { get; init; } = string.Empty;
        
        public string? TeamAbbreviation { get; init; }
        
        public IReadOnlyList<AthleteGameLogCategoryViewModel> Categories { get; init; } = [];
    }

    public sealed class AthleteGameLogCategoryViewModel
    {
        public string DisplayName { get; init; } = string.Empty;
        
        public IReadOnlyList<AthleteGameLogGameViewModel> Games { get; init; } = [];
        public IReadOnlyList<string> Totals { get; init; } = [];
    }

    public sealed class AthleteGameLogGameViewModel
    {
        public string EventId { get; init; } = string.Empty;
        
        public DateTimeOffset? GameDate { get; init; }
        
        public string DateDisplay { get; init; } = string.Empty;
        public string Result { get; init; } = string.Empty;
        public string Score { get; init; } = string.Empty;
        public string AtVs { get; init; } = string.Empty;
        
        public string? EventNote { get; init; }
        
        public string OpponentId { get; init; } = string.Empty;
        public string OpponentName { get; init; } = string.Empty;
        public string OpponentAbbreviation { get; init; } = string.Empty;
        
        public string? OpponentLogo { get; init; }
        
        public IReadOnlyList<string> Stats { get; init; } = [];
        
        public bool IsWin => string.Equals(Result, "W", StringComparison.OrdinalIgnoreCase);
        public bool IsLoss => string.Equals(Result, "L", StringComparison.OrdinalIgnoreCase);
    }
}