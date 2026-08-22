using SportsTracker.App.Enums;
using SportsTracker.App.Models;

namespace SportsTracker.App.ViewModels.SearchInfo
{
    public sealed class SearchPageViewModel
    {
        public string Query { get; init; } = string.Empty;
        
        public string? DidYouMean { get; init; }
        
        public IReadOnlyList<SearchResultViewModel> Players { get; init; } = [];
        public IReadOnlyList<SearchResultViewModel> Teams { get; init; } = [];
        public IReadOnlyList<SearchResultViewModel> Games { get; init; } = [];
        
        public int TotalResults => Players.Count + Teams.Count + Games.Count;
        
        public bool HasResults => TotalResults > 0;
        public bool HasPlayers => Players.Count > 0;
        public bool HasTeams => Teams.Count > 0;
        public bool HasGames => Games.Count > 0;
    }

    public sealed class SearchResultViewModel
    {
        public SearchResultType Type { get; init; }
        
        public string Id { get; init; } = string.Empty;
        
        public League League { get; init; }
        
        public string DisplayName { get; init; } = string.Empty;
        
        public string? Subtitle { get; init; }
        public string? Description { get; init; }
        public string? Image { get; init; }
        public string? DarkImage { get; init; }
        
        public DateTimeOffset? Date { get; init; }

        public string? DateDisplay => Date?.ToLocalTime().ToString("MMM d, yyyy h:mm tt");
        
        public bool HasImage => !string.IsNullOrWhiteSpace(Image);
    }

    public sealed class SearchSectionViewModel
    {
        public string Title { get; init; } = string.Empty;
        public string Icon { get; init; } = string.Empty;
        
        public IReadOnlyList<SearchResultViewModel> Results { get; init; } = [];
    }
}