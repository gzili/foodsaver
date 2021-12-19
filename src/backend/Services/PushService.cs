using backend.Hubs;
using backend.Models;
using Microsoft.AspNetCore.SignalR;

namespace backend.Services
{
    public class PushService : IPushService
    {
        private readonly IHubContext<ReservationsHub> _reservationsHub;

        public PushService(IHubContext<ReservationsHub> reservationsHub)
        {
            _reservationsHub = reservationsHub;
        }

        public void NotifyAvailableQuantityChanged(int id, decimal quantity)
        {
            _reservationsHub.Clients.Group(id.ToString()).SendAsync("AvailableQuantityChanged", quantity);
        }

        public void NotifyOffersChanged()
        {
            _reservationsHub.Clients.All.SendAsync("OffersChanged");
        }

        public void NotifyOfferDeleted(int id)
        {
            _reservationsHub.Clients.Group(id.ToString()).SendAsync("OfferDeleted");
        }

        public void NotifyReservationsChanged(Offer offer)
        {
            _reservationsHub.Clients.User(offer.Giver.Id.ToString()).SendAsync("ReservationsChanged", offer.Id);
        }

        public void NotifyReservationCompleted(Reservation reservation)
        {
            _reservationsHub.Clients.User(reservation.User.Id.ToString())
                .SendAsync("ReservationCompleted", reservation.Id);
        }
    }
}