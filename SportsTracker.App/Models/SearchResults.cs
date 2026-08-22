using SportsTracker.App.Enums;

namespace SportsTracker.App.Models
{
    public sealed class SearchResults
    {
        public string Query { get; init; } = string.Empty;
        
        public string? DidYouMean { get; init; }
        
        public IReadOnlyList<SearchResult> Players { get; init; } = [];
        public IReadOnlyList<SearchResult> Teams { get; init; } = [];
        public IReadOnlyList<SearchResult> Games { get; init; } = [];
        
        public int TotalResults => Players.Count + Teams.Count + Games.Count;
        
        public bool HasResults => TotalResults > 0;
    }

    public sealed class SearchResult
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
    }

    public enum SearchResultType
    {
        Player,
        Team,
        Game
    }
}