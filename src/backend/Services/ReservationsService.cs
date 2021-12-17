using System;
using System.Linq;
using backend.Data;
using backend.Exceptions;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class ReservationsService : IReservationsService
    {
        private readonly AppDbContext _db;
        
        private IQueryable<Reservation> Reservations =>  _db.Reservations
            .Include(r => r.User)
            .Include(r => r.Offer);

        public ReservationsService(AppDbContext db)
        {
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
            var random = new Random();
            reservation.Pin = (short) random.Next(1000, 10000);
            
            WithConcurrencyResolution(() =>
            {
                if (reservation.Quantity > reservation.Offer.AvailableQuantity)
                    throw new QuantityTooLargeException();
                
                reservation.Offer.AvailableQuantity -= reservation.Quantity;
                
                _db.Reservations.Add(reservation);
                _db.SaveChanges();
            });
        }

        public Reservation FindById(int id)
        {
            var reservation = Reservations.FirstOrDefault(r => r.Id == id);

            if (reservation == null)
            {
                throw new EntityNotFoundException("reservation", id);
            }

            return reservation;
        }

        public void Complete(Reservation reservation)
        {
            reservation.CompletedAt = DateTime.UtcNow;
            _db.SaveChanges();
        }

        public void Delete(Reservation reservation)
        {
            _db.Reservations.Remove(reservation);
            
            WithConcurrencyResolution(() =>
            {
                reservation.Offer.AvailableQuantity += reservation.Quantity;
                _db.SaveChanges();
            });
        }
    }
}