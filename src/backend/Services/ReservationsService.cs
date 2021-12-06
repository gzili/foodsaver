using System;
using System.Linq;
using backend.Data;
using backend.Exceptions;
using backend.Models;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class ReservationsService : IReservationsService
    {
        private readonly IReservationsRepository _reservationsRepository;
        private readonly IPushService _pushService;
        private readonly AppDbContext _db;

        public ReservationsService(IReservationsRepository reservationsRepository, IPushService pushService, AppDbContext db)
        {
            _reservationsRepository = reservationsRepository;
            _pushService = pushService;
            _db = db;
        }

        private static void WithConcurrencyResolution(Action f)
        {
            var saved = false;
            while (!saved)
            {
                try
                {
                    f();
                    saved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Offer)
                        {
                            var dbValues = entry.GetDatabaseValues();
                            entry.OriginalValues.SetValues(dbValues);
                            entry.CurrentValues.SetValues(dbValues);
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "Concurrency conflicts are not handled for " + entry.Metadata.Name);
                        }
                    }
                }
            }
        }

        public void Create(Reservation reservation)
        {
            WithConcurrencyResolution(() =>
            {
                if (reservation.Quantity > reservation.Offer.AvailableQuantity)
                    throw new QuantityTooLargeException();
                
                reservation.Offer.AvailableQuantity -= reservation.Quantity;
                _reservationsRepository.Create(reservation); // calls SaveChanges() implicitly!
            });
            
            _pushService.NotifyAvailableQuantityChanged(reservation.Offer.Id, reservation.Offer.AvailableQuantity);
            _pushService.NotifyReservationsChanged(reservation.Offer);
        }

        public Reservation FindById(int id)
        {
            var reservation = _reservationsRepository.Items.FirstOrDefault(r => r.Id == id);

            if (reservation == null)
            {
                throw new EntityNotFoundException("reservation", id);
            }

            return reservation;
        }

        public void Complete(Reservation reservation)
        {
            reservation.CompletedAt = DateTime.UtcNow;
            
            _db.SaveChanges(); // Can this be avoided?
            _pushService.NotifyReservationsChanged(reservation.Offer);
        }

        public void Delete(Reservation reservation)
        {
            WithConcurrencyResolution(() =>
            {
                reservation.Offer.AvailableQuantity += reservation.Quantity;
                _reservationsRepository.Delete(reservation); // calls SaveChanges() implicitly!
            });
            
            _pushService.NotifyAvailableQuantityChanged(reservation.Offer.Id, reservation.Offer.AvailableQuantity);
            _pushService.NotifyReservationsChanged(reservation.Offer);
        }
    }
}