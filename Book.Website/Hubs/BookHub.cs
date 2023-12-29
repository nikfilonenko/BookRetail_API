using Microsoft.AspNetCore.SignalR;

namespace Book.Website.Hubs;

public class BookHub : Hub
{
    public async Task NotifyWebUsers(string user, string message) {
        await Clients.All.SendAsync("DisplayNotification", user, message);
    }
}