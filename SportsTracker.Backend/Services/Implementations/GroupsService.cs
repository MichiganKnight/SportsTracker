using Microsoft.Extensions.Options;
using SportsTracker.Backend.Cache;
using SportsTracker.Backend.Config;
using SportsTracker.Backend.Integrations.ESPN;
using SportsTracker.Backend.Integrations.ESPN.DTOs.Groups;
using SportsTracker.Backend.Integrations.ESPN.Endpoints;
using SportsTracker.Backend.Integrations.ESPN.Mappers;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.Groups;

namespace SportsTracker.Backend.Services.Implementations
{
    public sealed class GroupsService : IGroupsService
    {
        private readonly IEspnApiClient _espnApiClient;
        private readonly ICacheService _cache;
        private readonly CacheOptions _cacheOptions;
        private readonly ILogger<GroupsService> _logger;

        public GroupsService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<GroupsService> logger)
        {
            _espnApiClient = espnApiClient;
            _cache = cache;
            _cacheOptions = cacheOptions.Value;
            _logger = logger;
        }

        public async Task<IReadOnlyList<SportsGroup>?> GetGroupsAsync(League league, CancellationToken cancellationToken = default)
        {
            string cacheKey = CacheKeys.Groups(league);
            
            IReadOnlyList<SportsGroup>? cached = await _cache.GetAsync<IReadOnlyList<SportsGroup>>(cacheKey);

            if (cached is not null)
            {
                return cached;
            }
            
            _logger.LogInformation("Fetching {League} Groups...", league);

            string endpoint = EspnEndpoints.Groups(league);

            ApiResult<GroupsResponseDto> result = await _espnApiClient.GetAsync<GroupsResponseDto>(endpoint, cancellationToken);

            if (!result.Success || result.Value is null)
            {
                _logger.LogWarning("Unable to Fetch {League} Groups: {Message}", league, result.Error?.Message);
                
                return null;
            }

            IReadOnlyList<SportsGroup> groups = GroupsMapper.Map(result.Value);
            
            await _cache.SetAsync(cacheKey, groups, TimeSpan.FromMinutes(_cacheOptions.GroupsMinutes));
            
            return groups;
        }
    }
}