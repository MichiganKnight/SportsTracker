using Microsoft.Extensions.Options;
using SportsTracker.App.Cache;
using SportsTracker.App.Config;
using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN;
using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Integrations.ESPN.Mappers;
using SportsTracker.App.Models.Groups;

namespace SportsTracker.App.Services
{
    public interface IGroupsService
    {
        Task<IReadOnlyList<SportsGroup>?> GetGroupsAsync(League league, CancellationToken cancellationToken = default);
    }
    
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