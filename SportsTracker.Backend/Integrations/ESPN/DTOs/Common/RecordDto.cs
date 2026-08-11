namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Common
{
    public sealed class RecordDto
    {
        public string? Name { get; init; }
        public string? Abbreviation { get; init; }
        public string? Type { get; init; }

        public string Summary { get; init; } = string.Empty;
        public string? DisplayValue { get; init; }
    }
}