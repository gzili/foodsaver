using System.Collections.Generic;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class OffersService
    {
        private readonly OffersRepository _offersRepository;

        public OffersService(OffersRepository offersRepository)
        {
            _offersRepository = offersRepository;
        }
        
        public void Save(Offer offer)
        {
            _offersRepository.Save(offer);
        }

        public Offer GetById(int id)
        {
            return _offersRepository.GetById(id);
        }

        public List<Offer> GetAll()
        {
            return _offersRepository.GetAll();
        }

        public List<Offer> GetAllActiveOffers()
        {
            return _offersRepository.GetAllActive();
        }
    }
}