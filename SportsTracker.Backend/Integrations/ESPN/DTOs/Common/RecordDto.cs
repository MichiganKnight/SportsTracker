namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Common
{
    public sealed class RecordDto
    {
        public string Summary { get; init; } = string.Empty;
        
        public string? DisplayValue { get; init; }
    }
}