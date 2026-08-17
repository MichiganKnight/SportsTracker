using Microsoft.Extensions.Options;
using SportsTracker.App.Cache;
using SportsTracker.App.Config;
using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN;
using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Integrations.ESPN.Mappers;
using SportsTracker.App.Models.GameDetails;

namespace SportsTracker.App.Services
{
    public interface IGameDetailsService
    {
        Task<GameDetails?> GetGameDetailsAsync(League league, string gameId, CancellationToken cancellationToken = default);
    }
    
    public sealed class GameDetailsService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<GameDetailsService> logger) : EspnCachedServiceBase(espnApiClient, cache), IGameDetailsService
    {
        private readonly CacheOptions _cacheOptions = cacheOptions.Value;

        public Task<GameDetails?> GetGameDetailsAsync(League league, string gameId, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<GameDetailsResponseDto, GameDetails>(league, $"Game Details for {gameId}", CacheKeys.GameDetails(league, gameId), EspnEndpoints.GameDetails(league, gameId), dto => GameDetailsMapper.Map(dto, league),
                details =>
                {
                    if (details.IsLive)
                    {
                        return TimeSpan.FromSeconds(_cacheOptions.GameDetailsLiveSeconds);
                    }

                    if (details.IsFinal)
                    {
                        return TimeSpan.FromMinutes(_cacheOptions.GameDetailsFinalMinutes);
                    }

                    return TimeSpan.FromMinutes(_cacheOptions.GameDetailsScheduledMinutes);
                }, logger, cancellationToken);
        }
    }
}