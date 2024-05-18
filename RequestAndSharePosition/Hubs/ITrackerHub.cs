using RequestAndSharePosition.Shared;

namespace RequestAndSharePosition.Hubs
{
    public interface ITrackerHub
    {
        Task ReceivePositionUpdatesAsync(Position position);
        Task StreamedPositionUpdatesAsync(Position position);
        Task RequestPositionAsync(string message);
        Task RequestAcceptedAsync(Guid groupId);
        Task RequestAcceptedAsync(bool sucess, string message);
        Task RequestSentAsync(bool success, string message);
        Task AcceptanceFailedAsync(bool success, string message);
    }
}
