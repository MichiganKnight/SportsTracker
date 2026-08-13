using Microsoft.Extensions.Options;
using SportsTracker.Backend.Cache;
using SportsTracker.Backend.Config;
using SportsTracker.Backend.Integrations.ESPN;
using SportsTracker.Backend.Integrations.ESPN.DTOs.GameDetails;
using SportsTracker.Backend.Integrations.ESPN.Endpoints;
using SportsTracker.Backend.Integrations.ESPN.Mappers;
using SportsTracker.Backend.Services.Base;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.GameDetails;

namespace SportsTracker.Backend.Services.Implementations
{
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