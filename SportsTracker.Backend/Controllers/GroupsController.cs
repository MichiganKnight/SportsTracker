using Microsoft.AspNetCore.Mvc;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.Groups;

namespace SportsTracker.Backend.Controllers
{
    [Route("api/v1/groups")]
    public sealed class GroupsController(IGroupsService groupsService) : ApiControllerBase
    {
        [HttpGet("{league}")]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<SportsGroup>>>> GetGroups(League league, CancellationToken cancellationToken)
        {
            IReadOnlyList<SportsGroup>? groups = await groupsService.GetGroupsAsync(league, cancellationToken);
            
            return groups is null ? NotFound() : ApiOk<IReadOnlyList<SportsGroup>>(groups);
        }
    }
}