using System.Text.Json;
using SportsTracker.App.Common;

namespace SportsTracker.App.Integrations.ESPN
{
    public interface IEspnApiClient
    {
        Task<ApiResult<T>> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default);
    }
    
    public sealed class EspnApiClient(HttpClient httpClient, ILogger<EspnApiClient> logger) : IEspnApiClient
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        public async Task<ApiResult<T>> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("GET {Endpoint}", endpoint);

                using HttpResponseMessage response = await httpClient.GetAsync(endpoint, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResult<T>.Fail(new Error("HTTP_ERROR", response.ReasonPhrase ?? "HTTP Request Failed"), (int)response.StatusCode);
                }

                await using Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);

                T? dto = await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions, cancellationToken);

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
                logger.LogError(ex, "Unexpected ESPN Error");
                
                return ApiResult<T>.Fail(new Error("EXCEPTION", ex.Message));
            }
        }
    }
}