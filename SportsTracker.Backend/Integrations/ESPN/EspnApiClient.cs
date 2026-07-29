using System.Text.Json;
using Microsoft.Extensions.Options;
using SportsTracker.Backend.Config;
using SportsTracker.Shared.Common;

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

        public async Task<ApiResult<T>> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("GET {Endpoint}", endpoint);

                using HttpResponseMessage response = await _httpClient.GetAsync(endpoint, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResult<T>.Fail(new Error("HTTP_ERROR", response.ReasonPhrase ?? "HTTP Request Failed"), (int)response.StatusCode);
                }

                await using Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);

                T? dto = await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions, cancellationToken);

                if (dto is null)
                {
                    return ApiResult<T>.Fail(new Error("DESERIALIZATION", "Unable to Deserialize ESPN Response"));
                }

                return ApiResult<T>.Ok(dto, (int)response.StatusCode);
            }
            catch (TaskCanceledException)
            {
                return ApiResult<T>.Fail(new Error("TIMEOUT", "The Request Timed Out"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected EXPN Error");
                
                return ApiResult<T>.Fail(new Error("EXCEPTION", ex.Message));
            }
        }
    }
}