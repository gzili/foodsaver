using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace backend.Hubs
{
    public class ReservationsHub : Hub
    {
        public async Task ListenOffer(string id)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, id);
        }
    }
}