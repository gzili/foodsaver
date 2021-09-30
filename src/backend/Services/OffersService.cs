using backend.Models;
using backend.Repositories;
using System.Collections.Generic;

namespace backend.Services
{
    public class OffersService : IService<Offer>
    {

        private readonly OffersRepository _offersRepository;
        public OffersService()
        {
            _offersRepository = new OffersRepository();
        }

        public Offer GetById( int id)
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
    }
}
