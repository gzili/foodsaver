using System;
using System.Collections.Generic;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class OffersService : IService<Offer>
    {
        private readonly OffersRepository _offersRepository;

        public OffersService()
        {
            _offersRepository = new OffersRepository();
        }

        public Offer GetById(int id)
        {
            return _offersRepository.GetById(id);
        }

        public List<Offer> GetAll()
        {
            return _offersRepository.GetAll();
        }

        public void Save(Offer offer)
        {
            _offersRepository.Save(offer);
        }

        public List<Offer> GetAllActiveOffers()
        {
            var list = new List<Offer>();
            foreach (var offer in GetAll())
                if (offer.ExpirationDate > DateTime.Now)
                    list.Add(offer);

            return list;
        }
    }
}