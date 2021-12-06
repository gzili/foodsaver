using System.Linq;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class ReservationsRepository : IReservationsRepository
    {
        private readonly AppDbContext _db;
        
        public IQueryable<Reservation> Items =>
            _db.Reservations
            .Include(r => r.User)
            .Include(r => r.Offer);

        public ReservationsRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Create(Reservation reservation)
        {
            _db.Reservations.Add(reservation);
            _db.SaveChanges();
        }

        public void Delete(Reservation reservation)
        {
            _db.Reservations.Remove(reservation);
            _db.SaveChanges();
        }
    }
}