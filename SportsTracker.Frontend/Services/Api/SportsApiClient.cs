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

        public async Task<ApiResponse<IReadOnlyList<Game>>?> GetScoreboardAsync(League league, CancellationToken cancellationToken = default) 
        { 
            IReadOnlyList<Game>? games = await GetAsync<IReadOnlyList<Game>>($"scoreboard/{league}", cancellationToken); 

            if (games is null)
            {
                return null;
            }
            
            return new ApiResponse<IReadOnlyList<Game>>
            {
                Data = games,
                TimestampUtc = DateTime.UtcNow,
                Version = "v1"
            };
        }

        private async Task<T?> GetAsync<T>(string relativeUrl, CancellationToken cancellationToken = default) 
        { 
            return await _httpClient.GetFromJsonAsync<T>(relativeUrl, cancellationToken); 
        } 

        /*public Task<ApiResponse<IReadOnlyList<Game>>?> GetScoreboardAsync(League league, CancellationToken cancellationToken = default)
        {
            return GetAsync<ApiResponse<IReadOnlyList<Game>>>($"scoreboard/{league}", cancellationToken);
        }

        private async Task<T?> GetAsync<T>(string relativeUrl, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<T>(relativeUrl, cancellationToken);
        }*/
    }
}