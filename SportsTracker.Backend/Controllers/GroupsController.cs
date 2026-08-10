using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.Groups;

namespace SportsTracker.Backend.Controllers
{
    [ApiController]
    [Route("api/v1/groups")]
    public sealed class GroupsController : Controller
    {
        private readonly IGroupsService _groupsService;
        
        public GroupsController(IGroupsService groupsService)
        {
            _groupsService = groupsService;
        }

        [HttpGet("{league}")]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<Group>>>> GetGroups(League league, CancellationToken cancellationToken)
        {
            IReadOnlyList<SportsGroup>? groups = await _groupsService.GetGroupsAsync(league, cancellationToken);
            
            if (groups is null)
            {
                return NotFound();
            }
            
            return Ok(new ApiResponse<IReadOnlyList<SportsGroup>>
            {
                Data = groups,
                TimestampUtc = DateTime.UtcNow,
                Version = "v1"
            });
        }
    }
}