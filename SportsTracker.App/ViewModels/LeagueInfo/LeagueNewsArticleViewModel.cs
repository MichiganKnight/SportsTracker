using SportsTracker.App.Enums;

namespace SportsTracker.App.ViewModels.LeagueInfo
{
    public sealed class LeagueNewsArticleViewModel
    {
        public League League { get; init; }
        
        public string Id { get; init; } = string.Empty;
        
        public string Type { get; init; } = string.Empty;
        public string Headline { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Byline { get; init; } = string.Empty;
        
        public DateTime? Published { get; init; }
        
        public bool Premium { get; init; }
        
        public string ArticleUrl { get; init; } = string.Empty;
        public string ImageUrl { get; init; } = string.Empty;
        public string ImageAlt { get; init; } = string.Empty;
        public string ImageCredit { get; init; } = string.Empty;

        public bool IsMedia => string.Equals(Type, "Media", StringComparison.OrdinalIgnoreCase);
        public bool HasImage => !string.IsNullOrWhiteSpace(ImageUrl);
        public bool HasDescription => !string.IsNullOrWhiteSpace(Description);
        public bool HasByline => !string.IsNullOrWhiteSpace(Byline);
    }
}