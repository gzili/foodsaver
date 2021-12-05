using System;
using System.Linq;
using backend.DTO;
using backend.DTO.Offer;
using backend.Exceptions;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class OffersService : IOffersService
    {
        private readonly IOffersRepository _offersRepository;
        private readonly IFileService _fileService;

        public OffersService(IOffersRepository offersRepository, IFileService fileService)
        {
            _offersRepository = offersRepository;
            _fileService = fileService;
        }

        public void Create(Offer offer)
        {
            _offersRepository.Create(offer);
        }

        public Offer FindById(int id)
        {
            var offer = _offersRepository.Items.FirstOrDefault(o => o.Id == id);

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
                ? _offersRepository.Items
                : _offersRepository.Items.Where(o => o.ExpiresAt > DateTime.Now);

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

            _fileService.DeleteFile(imagePath);
        }
    }

}