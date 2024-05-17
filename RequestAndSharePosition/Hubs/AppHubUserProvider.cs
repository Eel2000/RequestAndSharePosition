using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace RequestAndSharePosition.Hubs
{
    public sealed class AppHubUserProvider(ILogger<AppHubUserProvider> logger) : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            var auth = connection.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            logger.LogInformation($"User {auth} connected.");
            return auth;
        }
    }
}
