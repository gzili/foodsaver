using backend.Hubs;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class ReservationsService
    {
        private readonly ReservationsRepository _reservationsRepository;
        private readonly OfferEvents _offerEvents;

        public ReservationsService(ReservationsRepository reservationsRepository, OfferEvents offerEvents)
        {
            _reservationsRepository = reservationsRepository;
            _offerEvents = offerEvents;
        }

        private void NotifyAvailableQuantityChanged(Reservation reservation)
        {
            _offerEvents.OnAvailableQuantityChanged(
                new AvailableQuantityChangedEventArgs(reservation.Offer.Id, reservation.Offer.AvailableQuantity));
        }
        
        public void Save(Reservation reservation)
        {
            _reservationsRepository.Save(reservation);
            
            NotifyAvailableQuantityChanged(reservation);
        }
        
        public void Delete(Reservation reservation)
        {
            _reservationsRepository.Delete(reservation);
            
            NotifyAvailableQuantityChanged(reservation);
        }
    }
}