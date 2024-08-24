using Microsoft.AspNetCore.SignalR;

namespace DevConnect.Services
{
    public class UserIdService : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User.Identity!.Name;
        }
    }
}
