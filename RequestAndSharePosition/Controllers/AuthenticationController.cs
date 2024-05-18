using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RequestAndSharePosition.Services;
using RequestAndSharePosition.Shared;
using System.Security.Claims;

namespace RequestAndSharePosition.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController(IAuthenticationService authentication) : ControllerBase
    {
        [HttpPost("login")]
        public async ValueTask<IActionResult> LoginAsync([FromBody] LoginRequest login)
        {
            var result = await authentication.SinIngAsync(login);
            return Ok(result);
        }

        [HttpPost("register")]
        public async ValueTask<IActionResult> RegisterAsync([FromBody] LoginRequest register)
        {
            var result = await authentication.SignUpAsync(register);
            return Ok(result);
        }

        [HttpGet("info"), Authorize]
        public async ValueTask<IActionResult> InfoAsync()
        {
            if (HttpContext.User.Identity?.IsAuthenticated is not true)
            {
                return Unauthorized(new
                {
                    status = false,
                    message = "You're not authenticated",
                });
            }

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok(await authentication.GetInfoAsync(userId));
        }

        [HttpGet("infos"), Authorize]
        public async ValueTask<IActionResult> GetInfosAsync()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new
                {
                    status = false,
                    message = "User not found",
                });
            }

            return Ok(await authentication.GetUserAsync(userId));
        }
    }
}
