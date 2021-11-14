using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public void Create(Offer offer)
        {
            _offersRepository.Create(offer);
        }

        public Offer FindById(int id)
        {
            return _offersRepository.FindByCondition(o => o.Id == id).FirstOrDefault();
        }

        public IEnumerable<Offer> FindAll()
        {
            return _offersRepository.FindAll().ToList();
        }

        public IEnumerable<Offer> FindAllActiveOffers()
        {
            return _offersRepository.FindByCondition(o => o.ExpiresAt > DateTime.Now).ToList();
        }

        public void UpdateOffer(Offer offer, UpdateOfferDto updateOfferDto, FoodDto foodDto)
        {
            _offersRepository.UpdateOffer(offer, updateOfferDto, foodDto);
        }

        public void Delete(Offer offer)
        {
            _offersRepository.Delete(offer);
        }
    }

}