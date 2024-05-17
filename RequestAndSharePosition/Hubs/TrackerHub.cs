using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RequestAndSharePosition.Data;
using RequestAndSharePosition.Shared;

namespace RequestAndSharePosition.Hubs;

[Authorize]
public sealed class TrackerHub(ApplicationDbContext dbContext, ILogger<TrackerHub> logger) : Hub<ITrackerHub>
{
    public async Task SendPositionRequestAsync(RequestPosition request)
    {
        try
        {
            var newRequest = new Request
            {
                Sender = request.Sender,
                Receiver = request.Receiver,
                Message = request.Message
            };

            await dbContext.Requests.AddAsync(newRequest);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"User {Context.UserIdentifier} requested position for group {request.Sender}.");
            await Clients.User(request.Receiver).RequestPositionAsync(message: request.Message);
            await Clients.Caller.RequestSentAsync(true, "Request sent successfully. Awaiting for the other to accept");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error while sending position request for group {request.Sender}.");
            await Clients.Caller.RequestSentAsync(false, "Error while sending request. Please try again later.");
        }
    }

    public async Task SendPositionAsync(Position position)
    {
        if (string.IsNullOrWhiteSpace(position.GroupId))
            return;

        await Clients.OthersInGroup(position.GroupId).ReceivePositionUpdatesAsync(position);
    }

    public async Task StreamPositionAsync(IAsyncEnumerable<Position> positions)
    {
        await foreach (var position in positions)
        {
            if (string.IsNullOrWhiteSpace(position.GroupId))
                continue;

            await Clients.OthersInGroup(position.GroupId).StreamedPositionUpdatesAsync(position);
        }
    }

    public async Task AcceptRequestAsync(Guid requestId)
    {
        try
        {
            var ops = await dbContext
                 .Requests
                 .Where(r => r.Id == requestId && r.IsActive && !r.Accepted)
                 .ExecuteUpdateAsync(r => r.SetProperty(r => r.Accepted, true));

            if (ops is 0)
            {
                logger.LogWarning($"Request {requestId} not found or already accepted.");
                await Clients.Caller.AcceptanceFailedAsync(false, "Request not found or already accepted.");
                return;
            }

            var request = await dbContext.Requests.FirstOrDefaultAsync(r => r.Id == requestId && r.IsActive && r.Accepted);

            if (request is null)
            {
                logger.LogWarning($"Request {requestId} not found or already accepted.");
                await Clients.Caller.AcceptanceFailedAsync(false, "Request not found or already accepted.");
                return;
            }

            var sharingGroup = new SharingGroup
            {
                Description = $"Group for request {requestId} to share location until the receiver reject it",
                Name = $"Group {requestId}",
                RequestId = requestId.ToString()
            };

            var entry = await dbContext.SharingGroups.AddAsync(sharingGroup);
            await dbContext.SaveChangesAsync();

            await Groups.AddToGroupAsync(request.Sender, sharingGroup.Id.ToString());
            await Groups.AddToGroupAsync(request.Receiver, sharingGroup.Id.ToString());

            await Clients.User(request.Sender).RequestAcceptedAsync(sharingGroup.Id);
            await Clients.Caller.RequestAcceptedAsync(true, string.Empty);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error while accepting request {requestId}.");
        }

    }


}

