using backend.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace backend.Services
{
    public class PushService
    {
        private readonly IHubContext<ReservationsHub> _reservationsHub;

        public PushService(IHubContext<ReservationsHub> reservationsHub)
        {
            _reservationsHub = reservationsHub;
        }

        public void NotifyOffersChanged()
        {
            _reservationsHub.Clients.All.SendAsync("OffersChanged");
        }

        public void NotifyOfferDeleted(int id)
        {
            _reservationsHub.Clients.Group(id.ToString()).SendAsync("OfferDeleted");
        }
    }
}