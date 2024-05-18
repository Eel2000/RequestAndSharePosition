
namespace RequestAndSharePosition.Services
{
    public interface ITrackingService
    {
        ValueTask<object> GetIncomingRequestsAsync(string userId);
        ValueTask<object> GetOutGoingRequestsAsync(string userId);
    }
}
