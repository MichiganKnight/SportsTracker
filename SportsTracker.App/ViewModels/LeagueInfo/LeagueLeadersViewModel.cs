using SportsTracker.App.Enums;

namespace SportsTracker.App.ViewModels.LeagueInfo
{
    public sealed class LeagueLeadersViewModel
    {
        public League League { get; init; }
        
        public string LeagueName { get; init; } = string.Empty;
        
        public int Season { get; init; }

        public string SeasonName { get; init; } = string.Empty;
        
        public IReadOnlyList<LeaderSectionViewModel> Sections { get; init; } = [];
    }

    public sealed class LeaderSectionViewModel
    {
        public string Title { get; init; } = string.Empty;
        
        public IReadOnlyList<LeaderCategoryViewModel> Categories { get; init; } = [];
    }

    public sealed class LeaderCategoryViewModel
    {
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public IReadOnlyList<LeaderRowViewModel> Leaders { get; init; } = [];
    }

    public sealed class LeaderRowViewModel
    {
        public int Rank { get; init; }
        
        public string DisplayValue { get; init; } = string.Empty;
        
        public string AthleteId { get; init; } = string.Empty;
        public string AthleteName { get; init; } = string.Empty;
        
        public string? Headshot { get; init; }
        
        public string? TeamId { get; init; }
        
        public string? TeamName { get; init; }
        public string? TeamAbbreviation { get; init; }
        
        public string? TeamLogo { get; init; }
    }
}