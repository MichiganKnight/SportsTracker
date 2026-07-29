namespace SportsTracker.Shared.Common
{
    public sealed class ApiResponse<T>
    {
        public required T Data { get; init; }
        
        public DateTime TimestampUtc { get; init; } = DateTime.UtcNow;

        public string Version { get; init; } = "v1";
    }
}