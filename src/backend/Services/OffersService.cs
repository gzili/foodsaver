using System.Collections.Generic;
using backend.DTO.Offers;
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

        public void UpdateOffer(Offer offer, UpdateOfferDto updateOfferDto, FoodDto foodDto)
        {
            _offersRepository.UpdateOffer(offer, updateOfferDto, foodDto);
        }
    }

}