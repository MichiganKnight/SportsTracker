namespace SportsTracker.App.Models.LeagueNews
{
    public sealed class LeagueNews
    {
        public string Header { get; init; } = string.Empty;
        
        public IReadOnlyList<LeagueNewsArticle> Articles { get; init; } = [];
    }

    public sealed class LeagueNewsArticle
    {
        public string Id { get; init; } = string.Empty;

        public string Type { get; init; } = string.Empty;
        public string Headline { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Story { get; init; } = string.Empty;
        public string Byline { get; init; } = string.Empty;

        public DateTime? Published { get; init; }
        public DateTime? LastModified { get; init; }

        public bool Premium { get; init; }

        public string ArticleUrl { get; init; } = string.Empty;

        public IReadOnlyList<LeagueNewsImage> Images { get; init; } = [];
        public IReadOnlyList<LeagueNewsReference> References { get; init; } = [];
    }
    
    public sealed class LeagueNewsImage
    {
        public string Url { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Alt { get; init; } = string.Empty;
        public string Caption { get; init; } = string.Empty;
        public string Credit { get; init; } = string.Empty;
        
        public int? Width { get; init; }
        public int? Height { get; init; }
    }
    
    public sealed class LeagueNewsReference
    {
        public string Type { get; init; } = string.Empty;
        public string Id { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;

        public string Url { get; init; } = string.Empty;
    }
}