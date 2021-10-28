using System;
using System.Collections.Generic;
using System.Linq;
using backend.Data;
using backend.Models;

namespace backend.Repositories
{
    public class OffersRepository
    {
        private readonly AppDbContext _db;

        public OffersRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Save(Offer offer)
        {
            _db.Offers.Add(offer);
            _db.SaveChanges();
        }

        public Offer GetById(int id)
        {
            return _db.Offers.Find(id);
        }

        public List<Offer> GetAllActive()
        {
            return _db.Offers.Where(offer => offer.ExpiresAt > DateTime.Now).ToList();
        }

        public List<Offer> GetAll()
        {
            return _db.Offers.ToList();
        }
    }
}