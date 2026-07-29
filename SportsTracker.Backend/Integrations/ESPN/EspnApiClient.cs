using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SportsTracker.Backend.Config;

namespace SportsTracker.Backend.Integrations.ESPN
{
    public sealed class EspnApiClient : IEspnApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ILogger<EspnApiClient> _logger;

        public EspnApiClient(HttpClient httpClient, IOptions<EspnOptions> options, ILogger<EspnApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl.TrimEnd('/') + "/");
            
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("GET {Endpoint}", endpoint);

            Stopwatch stopwatch = Stopwatch.StartNew();
            
            using HttpResponseMessage response = await _httpClient.GetAsync(endpoint, cancellationToken);
            
            stopwatch.Stop();
            
            _logger.LogInformation("GET {Endpoint} Returned {StatusCode} in {Elapsed} ms", endpoint, (int)response.StatusCode, stopwatch.ElapsedMilliseconds);

            response.EnsureSuccessStatusCode();
            
            await using Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            
            return await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions, cancellationToken);
        }
    }
}