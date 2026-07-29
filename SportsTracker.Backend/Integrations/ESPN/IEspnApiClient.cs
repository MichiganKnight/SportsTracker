using SportsTracker.Shared.Common;

namespace SportsTracker.Backend.Integrations.ESPN
{
    public interface IEspnApiClient
    {
        Task<ApiResult<T>> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default);
    }
}