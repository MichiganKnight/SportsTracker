using Microsoft.AspNetCore.Mvc;
using SportsTracker.Shared.Common;

namespace SportsTracker.Backend.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected ActionResult<ApiResponse<T>> ApiOk<T>(T data, DateTime? timestampUtd = null)
        {
            return Ok(new ApiResponse<T>
            {
                Data = data,
                TimestampUtc = timestampUtd ?? DateTime.UtcNow,
                Version = "v1"
            });
        }
    }
}