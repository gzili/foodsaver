using System;
using System.Collections.Generic;
using System.Linq;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

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

        public Reservation GetById(int id)
        {
            return _db.Reservations
                .Include(o => o.User)
                .Include(o => o.Offer)
                .FirstOrDefault(o => o.Id == id);
        }

        public List<Reservation> GetAll()
        {
            return _db.Reservations
                .Include(o => o.User)
                .Include(o => o.Offer)
                .ToList();
        }
    }
}