namespace SportsTracker.App.Integrations.ESPN.DTOs.Search
{
    public sealed class EspnSearchResponseDto
    {
        public int TotalFound { get; init; }
        
        public string? DidYouMean { get; init; }
        
        public List<EspnSearchResultTypeDto> ResultTypes { get; init; } = [];
        public List<EspnSearchGroupDto> Results { get; init; } = [];
    }

    public sealed class EspnSearchResultTypeDto
    {
        public int TotalFound { get; init; }
        
        public string? Type { get; init; }
        public string? DisplayName { get; init; }
    }

    public sealed class EspnSearchGroupDto
    {
        public string? Type { get; init; }
        
        public int TotalFound { get; init; }
        public int Page { get; init; }
        public int Limit { get; init; }
        
        public string? DisplayName { get; init; }
        
        public List<EspnSearchContentDto> Contents { get; init; } = [];
    }

    public sealed class EspnSearchContentDto
    {
        public string? Id { get; init; }
        public string? Uid { get; init; }
        public string? Guid { get; init; }
        
        public string? Type { get; init; }
        public string? Status { get; init; }
        public string? EventId { get; init; }
        public string? DisplayName { get; init; }
        public string? Description { get; init; }
        public string? Subtitle { get; init; }
        public string? DefaultLeagueSlug { get; init; }
        public string? Sport { get; init; }
        
        public DateTimeOffset? Date { get; init; }
        
        public EspnSearchImageDto? Image { get; init; }
    }
    
    public sealed class EspnSearchImageDto
    {
        public string? Default { get; init; }
        public string? DefaultDark { get; init; }
    }
}