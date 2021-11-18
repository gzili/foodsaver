using System;
using System.Linq;
using backend.Data;
using backend.Exceptions;
using backend.Hubs;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class ReservationsService
    {
        private readonly PushService _pushService;
        private readonly ReservationsRepository _reservationsRepository;
        private readonly OfferEvents _offerEvents;
        private readonly AppDbContext _db;

        public ReservationsService(PushService pushService, ReservationsRepository reservationsRepository, OfferEvents offerEvents, AppDbContext db)
        {
            _pushService = pushService;
            _reservationsRepository = reservationsRepository;
            _offerEvents = offerEvents;
            _db = db;
        }

        private void NotifyAvailableQuantityChanged(Reservation reservation)
        {
            _offerEvents.OnAvailableQuantityChanged(
                new AvailableQuantityChangedEventArgs(reservation.Offer.Id, reservation.Offer.AvailableQuantity));
        }
        
        public void Create(Reservation reservation)
        {
            _reservationsRepository.Create(reservation);
            
            NotifyAvailableQuantityChanged(reservation);
            _pushService.NotifyReservationsChanged(reservation.Offer);
        }

        public Reservation FindById(int id)
        {
            var reservation = _reservationsRepository
                .FindByCondition(r => r.Id == id)
                .FirstOrDefault();

            if (reservation == null)
            {
                throw new EntityNotFoundException("reservation", id);
            }

            return reservation;
        }
        
        public void Delete(Reservation reservation)
        {
            _reservationsRepository.Delete(reservation);
            
            NotifyAvailableQuantityChanged(reservation);
            _pushService.NotifyReservationsChanged(reservation.Offer);
        }

        public void Complete(Reservation reservation)
        {
            reservation.CompletedAt = DateTime.UtcNow;
            
            _db.SaveChanges();
            _pushService.NotifyReservationsChanged(reservation.Offer);
        }
    }
}