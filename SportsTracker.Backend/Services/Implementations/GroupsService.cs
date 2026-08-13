using Microsoft.Extensions.Options;
using SportsTracker.Backend.Cache;
using SportsTracker.Backend.Config;
using SportsTracker.Backend.Integrations.ESPN;
using SportsTracker.Backend.Integrations.ESPN.DTOs.Groups;
using SportsTracker.Backend.Integrations.ESPN.Endpoints;
using SportsTracker.Backend.Integrations.ESPN.Mappers;
using SportsTracker.Backend.Services.Base;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.Groups;

namespace SportsTracker.Backend.Services.Implementations
{
    public sealed class GroupsService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<GroupsService> logger) : EspnCachedServiceBase(espnApiClient, cache), IGroupsService
    {
        private readonly CacheOptions _cacheOptions = cacheOptions.Value;

        public Task<IReadOnlyList<SportsGroup>?> GetGroupsAsync(League league, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<GroupsResponseDto, IReadOnlyList<SportsGroup>>(league, "Groups", CacheKeys.Groups(league), EspnEndpoints.Groups(league), dto => GroupsMapper.Map(dto),
                _ => TimeSpan.FromMinutes(_cacheOptions.GroupsMinutes), logger, cancellationToken);
        }
    }
}