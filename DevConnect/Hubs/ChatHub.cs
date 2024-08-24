using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DevConnect.Hubs
{
    [Authorize]
    public class ChatHub:Hub
    {
        public async Task SendAsync(string toUser, int chatId)
        {
            if (Context.User.Identity.Name is string fromUser)
            {
                await Clients.Users(toUser, fromUser).SendAsync("ReceiveUpdate", chatId);
            }
        }
    }
}
