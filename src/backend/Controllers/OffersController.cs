using System.Collections.Generic;
using System.Linq;
using backend.DTO.Offers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // "api/offers"
    public class OffersController : ControllerBase
    {
        private readonly OffersService _offersService;

        public OffersController()
        {
            _offersService = new OffersService();
        }

        [HttpGet] // "api/offers"
        public IEnumerable<OfferDto> FindAll()
        {
            return _offersService.GetAll().Select(ToDto);
        }

        [HttpGet("{id:int}")] // "api/offers/<number>"
        public ActionResult<OfferDto> FindById(int id)
        {
            var offer = _offersService.GetById(id);
            return offer != null ? Ok(ToDto(offer)) : NotFound();
        }

        [Authorize]
        [HttpPost("add")] // "api/offers/add"
        public void Create(Offer offer)
        {
            _offersService.Save(offer);
        }

        [HttpGet("byUser")]
        public Dictionary<int, IEnumerable<Offer>> FindGrouped()
        {
            return _offersService.GetGrouped();
        }

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