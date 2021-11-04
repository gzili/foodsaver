using System;
using System.Collections.Generic;
using System.Linq;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class PickupsRepository
    {
        private readonly AppDbContext _db;

        public PickupsRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Save(Pickup pickup)
        {
            _db.Pickups.Add(pickup);
            _db.SaveChanges();
        }

        public Pickup GetById(int id)
        {
            return _db.Pickups
                .Include(o => o.Reservation)
                .FirstOrDefault(o => o.Id == id);
        }

        public List<Pickup> GetAll()
        {
            return _db.Pickups
                .Include(o => o.Reservation)
                .ToList();
        }
    }
}