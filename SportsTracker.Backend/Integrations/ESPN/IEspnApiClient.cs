namespace SportsTracker.Backend.Integrations.ESPN
{
    public interface IEspnApiClient
    {
        Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default);
    }
}