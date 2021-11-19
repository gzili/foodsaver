using Microsoft.AspNetCore.SignalR;

namespace backend.Services
{
    public class UserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User.Identity.Name;
        }
    }
}