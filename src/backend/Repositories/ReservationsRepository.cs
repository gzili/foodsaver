using backend.Data;
using backend.Models;

namespace backend.Repositories
{
    public class ReservationsRepository
    {
        private readonly AppDbContext _db;

        public ReservationsRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Save(Reservation reservation)
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