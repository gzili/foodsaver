using System;
using System.Linq;
using System.Linq.Expressions;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class ReservationsRepository : IRepositoryBase<Reservation>
    {
        private readonly AppDbContext _db;

        public ReservationsRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Create(Reservation reservation)
        {
            _db.Reservations.Add(reservation);
            _db.SaveChanges();
        }

        public IQueryable<Reservation> FindAll()
        {
            return _db.Reservations
                .Include(r => r.User)
                .Include(r => r.Offer);
        }

        public IQueryable<Reservation> FindByCondition(Expression<Func<Reservation, bool>> expression)
        {
            return FindAll().Where(expression);
        }

        public void Delete(Reservation reservation)
        {
            _db.Reservations.Remove(reservation);
            _db.SaveChanges();
        }
    }
}