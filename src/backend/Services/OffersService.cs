using System;
using System.Collections.Generic;
using System.Linq;
using backend.DTO;
using backend.DTO.Offer;
using backend.Exceptions;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class OffersService
    {
        private readonly FileUploadService _fileUploadService;
        private readonly OffersRepository _offersRepository;
        private readonly PushService _pushService;

        public OffersService(
            FileUploadService fileUploadService,
            OffersRepository offersRepository,
            PushService pushService)
        {
            _fileUploadService = fileUploadService;
            _offersRepository = offersRepository;
            _pushService = pushService;
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

        public PaginatedList<Offer> FindAllPaginated(bool includeExpired, int page, int limit)
        {
            if (page < 0)
                page = 0;
            
            limit = limit switch
            {
                > 25 => 25,
                <= 0 => 5,
                _ => limit
            };
            
            var offers = includeExpired
                ? _offersRepository.FindAll()
                : _offersRepository.FindByCondition(o => o.ExpiresAt > DateTime.Now);

            var orderedOffers = offers.OrderByDescending(o => o.ExpiresAt);

            return PaginatedList<Offer>.Create(orderedOffers, page, limit);
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