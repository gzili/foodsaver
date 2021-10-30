using System;
using System.Collections.Generic;
using backend.DTO.Address;
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

        public Offer SaveDto(CreateOfferDto offerCreateDto, string imagePath, User user)
        {
            var offer = new Offer
            {
                Food = new Food
                {
                    Name = offerCreateDto.FoodName,
                    Unit = offerCreateDto.FoodUnit,
                    ImagePath = imagePath
                },
                CreatedAt = DateTime.Now,
                Description = offerCreateDto.Description,
                ExpiresAt = offerCreateDto.ExpirationDate,
                Quantity = offerCreateDto.Quantity,
                Giver = user
            };
            
            _offersRepository.Save(offer);
            return offer; // returns saved offer with id field generated
        }

        public OfferDto ToDto(Offer offer) => new()
        {
            Id = offer.Id,
            Food = offer.Food,
            CreationDate = offer.CreatedAt,
            Description = offer.Description,
            ExpirationDate = offer.ExpiresAt,
            Quantity = offer.Quantity,
            Giver = new GiverDto
            {
                Id = offer.Giver.Id,
                Address = new AddressDto
                {
                    Street = offer.Giver.Address.Street,
                    City = offer.Giver.Address.City
                },
                Name = offer.Giver.Username
            }
        };
    }
}