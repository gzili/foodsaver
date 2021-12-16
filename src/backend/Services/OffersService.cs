using System;
using System.Linq;
using backend.Data;
using backend.DTO;
using backend.DTO.Offer;
using backend.Exceptions;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class OffersService : IOffersService
    {
        private readonly AppDbContext _db;
        private readonly IFileService _fileService;
        
        private IQueryable<Offer> Offers => _db.Offers
            .Include(o => o.Address)
            .Include(o => o.Food)
            .Include(o => o.Giver)
            .ThenInclude(u => u.Address);

        public OffersService(AppDbContext db, IFileService fileService)
        {
            _db = db;
            _fileService = fileService;
        }

        public void Create(Offer offer)
        {
            _db.Offers.Add(offer);
            _db.SaveChanges();
        }

        public Offer FindById(int id)
        {
            var offer = Offers.FirstOrDefault(o => o.Id == id);

            if (offer == null)
            {
                throw new EntityNotFoundException(nameof(offer), id);
            }

            return offer;
        }

        public PaginatedList<Offer> FindAllPaginated(bool includeExpired, int page, int limit, int userId)
        {
            if (page < 0)
                page = 0;
            
            limit = limit switch
            {
                > 25 => 25,
                <= 0 => 5,
                _ => limit
            };

            IQueryable<Offer> offers = Offers.OrderByDescending(o => o.CreatedAt);

            if (!includeExpired)
            {
                offers = offers.Where(o => o.ExpiresAt > DateTime.Now);
            }

            if (userId > 0)
            {
                offers = offers.Where(o => o.Giver.Id == userId);
            }
            
            return PaginatedList<Offer>.Create(offers, page, limit);
        }

        public void Update(Offer offer, UpdateOfferDto updateOfferDto, FoodDto foodDto)
        {
            _db.Entry(offer).CurrentValues.SetValues(updateOfferDto);
            _db.Entry(offer.Food).CurrentValues.SetValues(foodDto);
            _db.SaveChanges();
        }

        public void Delete(Offer offer)
        {
            var imagePath = offer.Food.ImagePath;

            _db.Offers.Remove(offer);
            _db.SaveChanges();

            _fileService.DeleteFile(imagePath);
        }
    }

}