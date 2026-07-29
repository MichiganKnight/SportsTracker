using System.Text.Json;
using Microsoft.Extensions.Options;
using SportsTracker.Backend.Config;

namespace SportsTracker.Backend.Integrations.ESPN
{
    public sealed class EspnApiClient : IEspnApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public EspnApiClient(HttpClient httpClient, IOptions<EspnOptions> options)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
            
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
        {
            using HttpResponseMessage response = await _httpClient.GetAsync(endpoint, cancellationToken);

            response.EnsureSuccessStatusCode();
            
            await using Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            
            return await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions, cancellationToken);
        }
    }
}