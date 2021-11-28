using System;
using System.Collections.Generic;
using System.Linq;
using backend.DTO.Offer;
using backend.Exceptions;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class OffersService
    {
        private readonly FileUploadService _fileUploadService;
        private readonly PushService _pushService;
        private readonly OffersRepository _offersRepository;

        public OffersService(FileUploadService fileUploadService, PushService pushService, OffersRepository offersRepository)
        {
            _fileUploadService = fileUploadService;
            _pushService = pushService;
            _offersRepository = offersRepository;
        }
        
        public void Create(Offer offer)
        {
            _offersRepository.Create(offer);
            
            _pushService.NotifyOffersChanged();
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

        public void Update(Offer offer, UpdateOfferDto updateOfferDto, FoodDto foodDto)
        {
            _offersRepository.Update(offer, updateOfferDto, foodDto);
        }

        public void Delete(Offer offer)
        {
            var imagePath = offer.Food.ImagePath;
            
            _offersRepository.Delete(offer);
            
            _pushService.NotifyOfferDeleted(offer.Id);
            _pushService.NotifyOffersChanged();
            
            _fileUploadService.DeleteFile(imagePath);
        }
    }

}