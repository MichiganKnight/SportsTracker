using Microsoft.Extensions.Options;
using SportsTracker.Frontend.Config;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;

namespace SportsTracker.Frontend.Services.Api
{
    public class SportsApiClient : ISportsApiClient
    {
        private readonly HttpClient _httpClient;

        public SportsApiClient(HttpClient httpClient, IOptions<SportsApiOptions> options)
        {
            _httpClient = httpClient;
            
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
        }

        public async Task<ApiResponse<CachedScoreboard>?> GetScoreboardAsync(League league, CancellationToken cancellationToken = default)
        {
            ApiResponse<CachedScoreboard>? response = await GetLeagueAsync(league, cancellationToken);

            if (response is null)
            {
                return null;
            }

            return new ApiResponse<CachedScoreboard>
            {
                Data = response.Data,
                TimestampUtc = response.TimestampUtc,
                Version = response.Version
            };
        }

        public async Task<ApiResponse<CachedScoreboard>?> GetLeagueAsync(League league, CancellationToken cancellationToken = default)
        {
            return await GetAsync<ApiResponse<CachedScoreboard>>($"league/{league}", cancellationToken);
        }

        private async Task<T?> GetAsync<T>(string relativeUrl, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<T>(relativeUrl, cancellationToken);
        }
    }
}