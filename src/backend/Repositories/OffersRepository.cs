using System;
using System.Collections.Generic;
using System.Linq;
using backend.Data;
using backend.DTO.Offers;
using backend.Models;
using Microsoft.EntityFrameworkCore;

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
            return _db.Offers
                .Include(o => o.Address)
                .Include(o => o.Food)
                .Include(o => o.Giver)
                .ThenInclude(u => u.Address)
                .FirstOrDefault(o => o.Id == id);
        }

        public List<Offer> GetAllActive()
        {
            return _db.Offers
                .Where(offer => offer.ExpiresAt > DateTime.Now)
                .Include(o => o.Address)
                .Include(o => o.Food)
                .Include(o => o.Giver)
                .ThenInclude(u => u.Address)
                .ToList();
        }

        public List<Offer> GetAll()
        {
            var offers = _db.Offers
                .Include(o => o.Address)
                .Include(o => o.Food)
                .Include(o => o.Giver)
                .ThenInclude(u => u.Address)
                .ToList();
            return offers;
        }

        public Offer UpdateOffer (Offer oldOffer, UpdateOfferDto newOffer)
        {
            _db.Entry(oldOffer).CurrentValues.SetValues(newOffer);
            if(newOffer.FoodName != null) oldOffer.Food.Name = newOffer.FoodName;
            if(newOffer.FoodUnit != null) oldOffer.Food.Unit = newOffer.FoodUnit;
            
            _db.SaveChanges();
            return oldOffer;
        }
        
    }
}