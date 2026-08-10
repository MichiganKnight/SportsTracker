using Microsoft.Extensions.Options;
using SportsTracker.Frontend.Config;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;
using SportsTracker.Shared.Models.GameInfo;
using SportsTracker.Shared.Models.Standings;

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

        public async Task<ApiResponse<IReadOnlyList<Game>>?> GetScoreboardAsync(League league, CancellationToken cancellationToken = default)
        {
            ApiResponse<CachedScoreboard>? response = await GetLeagueAsync(league, cancellationToken);

            if (response is null)
            {
                return null;
            }

            return new ApiResponse<IReadOnlyList<Game>>
            {
                Data = response.Data.Games,
                TimestampUtc = response.TimestampUtc,
                Version = response.Version
            };
        }

        public async Task<ApiResponse<CachedScoreboard>?> GetLeagueAsync(League league, CancellationToken cancellationToken = default)
        {
            return await GetAsync<ApiResponse<CachedScoreboard>>($"scoreboard/{league}", cancellationToken);
        }

        public async Task<ApiResponse<LeagueStandings>?> GetStandingsAsync(League league, CancellationToken cancellationToken = default)
        {
            return await GetAsync<ApiResponse<LeagueStandings>>($"standings/{league}", cancellationToken);
        }

        private async Task<T?> GetAsync<T>(string relativeUrl, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<T>(relativeUrl, cancellationToken);
        }
    }
}