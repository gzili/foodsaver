using System;
using System.Collections.Generic;
using System.Linq;
using backend.Data;
using backend.DTO.Address;
using backend.DTO.Summary;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SummaryController : ControllerBase
    {
        private readonly AppDbContext _db;

        public SummaryController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("cities")]
        public IEnumerable<CitySummaryDto> Cities()
        {
            var offersByCity =
                _db.Offers
                    .GroupBy(o => o.Giver.Address.City)
                    .Select(offers => new CitySummaryDto
                        { City = offers.Key, OffersCount = offers.Count() });
            
            return offersByCity;
        }

        [HttpGet("givers")]
        public IEnumerable<GiverSummaryDto> Givers()
        {
            var giversSummary =
                _db.Users
                    .Select(u => new GiverSummaryDto
                    {
                        Id = u.Id,
                        Username = u.Username,
                        AvatarPath = u.AvatarPath,
                        Address = new AddressDto
                        {
                            Street = u.Address.Street,
                            City = u.Address.City
                        },
                        ActiveOffersCount = u.Offers.Count(o => o.ExpiresAt > DateTime.UtcNow)
                    })
                    .Where(a => a.ActiveOffersCount > 0);

            return giversSummary;
        }
    }
}