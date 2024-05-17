using RequestAndSharePosition.Data;
using RequestAndSharePosition.Shared;

namespace RequestAndSharePosition.Services
{
    public interface IAuthenticationService
    {
        ValueTask<object> GetInfoAsync(string userId);
        ValueTask<object> GetUserAsync();
        ValueTask<object> SignUpAsync(LoginRequest login);
        ValueTask<object> SinIngAsync(LoginRequest login);
    }
}
