using Microsoft.EntityFrameworkCore;
using RequestAndSharePosition.Data;

namespace RequestAndSharePosition.Services
{
    public class TrackingService(ApplicationDbContext dbContext) : ITrackingService
    {
        public async ValueTask<object> GetIncomingRequestsAsync(string userId)
        {
            var requests = await dbContext
                .Requests
                .Where(x => x.Receiver == userId && x.IsActive == true)
                .ToListAsync();


            return new
            {
                status = true,
                message = "List of all requests",
                Data = requests
            };
        }

        public async ValueTask<object> GetOutGoingRequestsAsync(string userId)
        {
            var requests = await dbContext
                .Requests
                .Where(x => x.Sender == userId)
                .ToListAsync();


            return new
            {
                status = true,
                message = "List of all requests",
                Data = requests
            };
        }
    }
}
