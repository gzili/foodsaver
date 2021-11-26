using System;
using System.Collections.Generic;
using System.Linq;
using backend.DTO.Offers;
using backend.Exceptions;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class OffersService
    {
        private readonly FileUploadService _fileUploadService;
        private readonly OffersRepository _offersRepository;

        public OffersService(FileUploadService fileUploadService, OffersRepository offersRepository)
        {
            _fileUploadService = fileUploadService;
            _offersRepository = offersRepository;
        }
        
        public void Create(Offer offer)
        {
            _offersRepository.Create(offer);
        }

        public Offer FindById(int id)
        {
            var offer = _offersRepository.FindByCondition(o => o.Id == id).FirstOrDefault();

            if (offer == null)
            {
                throw new EntityNotFoundException(nameof(offer), id);
            }

            return offer;
        }

        public IEnumerable<Offer> FindAll(bool includeExpired)
        {
            var offers = includeExpired
                ? _offersRepository.FindAll()
                : _offersRepository.FindByCondition(o => o.ExpiresAt > DateTime.Now);

            return offers.ToList();
        }

        public void UpdateOffer(Offer offer, UpdateOfferDto updateOfferDto, FoodDto foodDto)
        {
            _offersRepository.UpdateOffer(offer, updateOfferDto, foodDto);
        }

        public void Delete(Offer offer)
        {
            var imagePath = offer.Food.ImagePath;
            
            _offersRepository.Delete(offer);
            
            _fileUploadService.DeleteFile(imagePath);
        }
    }

}