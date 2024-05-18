using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RequestAndSharePosition.Services;
using System.Security.Claims;

namespace RequestAndSharePosition.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackingController(ITrackingService trackingService) : ControllerBase
    {
        [HttpGet("incoming-requests"), Authorize]
        public async ValueTask<IActionResult> GetIncomingRequestsAsync()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    status = false,
                    message = "User not found",
                });
            }

            return Ok(await trackingService.GetIncomingRequestsAsync(userId));
        }

        [HttpGet("outgoing-requests"), Authorize]
        public async ValueTask<IActionResult> GetOutGoingRequestsAsync()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    status = false,
                    message = "User not found",
                });
            }

            return Ok(await trackingService.GetOutGoingRequestsAsync(userId));
        }
    }
}
