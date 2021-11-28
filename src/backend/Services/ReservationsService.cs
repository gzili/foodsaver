using System;
using System.Linq;
using backend.Data;
using backend.Exceptions;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class ReservationsService
    {
        private readonly PushService _pushService;
        private readonly ReservationsRepository _reservationsRepository;
        private readonly AppDbContext _db;

        public ReservationsService(PushService pushService, ReservationsRepository reservationsRepository, AppDbContext db)
        {
            _pushService = pushService;
            _reservationsRepository = reservationsRepository;
            _db = db;
        }

        public void Create(Reservation reservation)
        {
            _reservationsRepository.Create(reservation);
            
            _pushService.NotifyAvailableQuantityChanged(reservation.Offer.Id, reservation.Offer.AvailableQuantity);
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
            
            _pushService.NotifyAvailableQuantityChanged(reservation.Offer.Id, reservation.Offer.AvailableQuantity);
            _pushService.NotifyReservationsChanged(reservation.Offer);
        }

        public void Complete(Reservation reservation)
        {
            reservation.CompletedAt = DateTime.UtcNow;
            
            _db.SaveChanges(); // Can this be avoided?
            _pushService.NotifyReservationsChanged(reservation.Offer);
        }
    }
}