using System.Linq;
using backend.Data;
using backend.DTO.Offer;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class OffersRepository : IOffersRepository
    {
        private readonly AppDbContext _db;

        public IQueryable<Offer> Items
        {
            get
            {
                return _db.Offers
                    .Include(o => o.Address)
                    .Include(o => o.Food)
                    .Include(o => o.Giver)
                    .ThenInclude(u => u.Address);
            }
        }

        public OffersRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Create(Offer offer)
        {
            _db.Offers.Add(offer);
            _db.SaveChanges();
        }

        public void Update (Offer offer, UpdateOfferDto updateOfferDto, FoodDto foodDto)
        {
            _db.Entry(offer).CurrentValues.SetValues(updateOfferDto);
            _db.Entry(offer.Food).CurrentValues.SetValues(foodDto);
            _db.SaveChanges();
        }

        public void Delete(Offer offer)
        {
            _db.Offers.Remove(offer);
            _db.SaveChanges();
        }
        
    }
}