using System;
using System.Collections.Generic;
using backend.DTO.Offers;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class OffersService : IService<Offer>
    {
        private readonly OffersRepository _offersRepository;
        
        private readonly FoodService _foodService;

        public OffersService(OffersRepository offersRepository, FoodService foodService)
        {
            _offersRepository = offersRepository;
            _foodService = foodService;
        }

        public Offer GetById(int id)
        {
            return _offersRepository.GetById(id);
        }

        public Dictionary<int, IEnumerable<Offer>> GetGrouped()
        {
            return _offersRepository.OffersByUser;
        }

        public List<Offer> GetAll()
        {
            
            return _offersRepository.GetAll();
        }

        public void Save(Offer offer)
        {
            _offersRepository.Save(offer);
        }
        
        public void SaveDto(OfferCreateDto offerCreateDto, User user)
        {
            _offersRepository.Save(GetOfferFromCreateDto(offerCreateDto, user));
        }

        public List<Offer> GetAllActiveOffers()
        {
            var list = new List<Offer>();
            foreach (var offer in GetAll())
                if (offer.ExpirationDate > DateTime.Now)
                    list.Add(offer);

            return list;
        }

        private Offer GetOfferFromCreateDto(OfferCreateDto offerCreateDto, User user) => new(GetAll().Count + 1)
        {
            Food = _foodService.GetFromDto(offerCreateDto.FoodDto),
            CreationDate = DateTime.Now,
            Description = offerCreateDto.Description,
            ExpirationDate = offerCreateDto.ExpirationDate,
            Quantity = offerCreateDto.Quantity,
            Giver = user
        };

        public OfferDto ToDto(Offer offer) => new()
        {
            Id = offer.Id,
            Food = offer.Food,
            CreationDate = offer.CreationDate,
            Description = offer.Description,
            ExpirationDate = offer.ExpirationDate,
            Quantity = offer.Quantity,
            Giver = new GiverDto
            {
                Id = offer.Giver.Id,
                Address = offer.Giver.Address,
                Name = offer.Giver.Name
            }
        };
    }
}