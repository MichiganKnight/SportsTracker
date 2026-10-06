namespace SportsTracker.App.Integrations.ESPN.DTOs.News
{
    public sealed class LeagueNewsResponseDto
    {
        public string? Header { get; init; }

        public List<LeagueNewsArticleDto> Articles { get; init; } = [];
    }

    public sealed class LeagueNewsArticleResponseDto
    {
        public int? ResultsCount { get; init; }

        public List<LeagueNewsArticleDto> Headlines { get; init; } = [];
    }

    public sealed class LeagueNewsArticleDto
    {
        public long? Id { get; init; }

        public string? Type { get; init; }
        public string? Headline { get; init; }
        public string? Description { get; init; }
        public string? Story { get; init; }
        public string? Byline { get; init; }

        public DateTime? Published { get; init; }
        public DateTime? LastModified { get; init; }

        public bool? Premium { get; init; }

        public List<LeagueNewsCategoryDto> Categories { get; init; } = [];
        public List<LeagueNewsImageDto> Images { get; init; } = [];

        public LeagueNewsArticleLinksDto? Links { get; init; }
    }
    
    public sealed class LeagueNewsCategoryDto
    {
        public string? Type { get; init; }
        public string? Description { get; init; }

        public long? AthleteId { get; init; }
        public long? TeamId { get; init; }
        public long? EventId { get; init; }

        public LeagueNewsCategoryTeamDto? Team { get; init; }
        public LeagueNewsCategoryAthleteDto? Athlete { get; init; }
        public LeagueNewsCategoryEventDto? Event { get; init; }
    }

    public sealed class LeagueNewsCategoryTeamDto
    {
        public long? Id { get; init; }
        
        public string? Description { get; init; }
        public string? Abbreviation { get; init; }
    }

    public sealed class LeagueNewsCategoryAthleteDto
    {
        public long? Id { get; init; }
        
        public string? Description { get; init; }
    }

    public sealed class LeagueNewsCategoryEventDto
    {
        public long? Id { get; init; }
        
        public string? Description { get; init; }
    }  
    
    public sealed class LeagueNewsImageDto
    {
        public string? Type { get; init; }
        public string? Caption { get; init; }
        public string? Alt { get; init; }
        public string? Credit { get; init; }
        
        public int? Width { get; init; }
        public int? Height { get; init; }
        
        public string? Url { get; init; }
    }

    public sealed class LeagueNewsArticleLinksDto
    {
        public LeagueNewsWebLinkDto? Web { get; init; }
    }

    public sealed class LeagueNewsWebLinkDto
    {
        public string? Href { get; init; }
    }
}