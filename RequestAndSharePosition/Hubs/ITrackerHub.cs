using RequestAndSharePosition.Shared;

namespace RequestAndSharePosition.Hubs
{
    public interface ITrackerHub
    {
        ValueTask ReceivePositionUpdatesAsync(Position position);
        ValueTask StreamedPositionUpdatesAsync(Position position);
        ValueTask RequestPositionAsync(string message);
        ValueTask RequestAcceptedAsync(Guid groupId);
        ValueTask RequestAcceptedAsync(bool sucess, string message);
        ValueTask RequestSentAsync(bool success, string message);
        ValueTask AcceptanceFailedAsync(bool success, string message);
    }
}
