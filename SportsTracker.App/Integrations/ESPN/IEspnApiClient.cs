using SportsTracker.App.Common;

namespace SportsTracker.App.Integrations.ESPN
{
    public interface IEspnApiClient
    {
        Task<ApiResult<T>> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default);
    }
}