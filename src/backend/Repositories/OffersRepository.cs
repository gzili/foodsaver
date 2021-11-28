using System;
using System.Linq;
using System.Linq.Expressions;
using backend.Data;
using backend.DTO.Offer;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class OffersRepository : IRepositoryBase<Offer>
    {
        private readonly AppDbContext _db;

        public OffersRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Create(Offer offer)
        {
            _db.Offers.Add(offer);
            _db.SaveChanges();
        }

        public IQueryable<Offer> FindAll()
        {
            return _db.Offers
                .Include(o => o.Address)
                .Include(o => o.Food)
                .Include(o => o.Giver)
                .ThenInclude(u => u.Address);
        }

        public IQueryable<Offer> FindByCondition(Expression<Func<Offer, bool>> expression)
        {
            return FindAll().Where(expression);
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