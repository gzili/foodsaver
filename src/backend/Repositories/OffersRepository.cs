using backend.Models;
using System.Collections.Generic;
using System;
using backend.Services;

namespace backend.Repositories
{
    public class OffersRepository
    {

        private readonly AppDbContext _appDbContext;
        public OffersRepository()
        {
            _appDbContext = AppDbContext.GetObject();
        }

        public void Save(Offer offer)
        {
            _appDbContext.Offers.Add(offer);
        }

        public Offer GetById(int id)
        {
            return _appDbContext.GetById(_appDbContext.Offers, id);
        }
        public List<Offer> GetAll()
        {
            return _appDbContext.Offers;
        }
    }
}
