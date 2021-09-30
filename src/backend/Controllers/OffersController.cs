using System.Collections.Generic;
using System.Linq;
using backend.DTO;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using backend.Services;

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
        public IEnumerable<OfferDto> Get()
        {
            // should return a list of all user-posted offers
            return _offersService.GetAll().Select(ToDto);
        }

        [HttpGet("{id:int}")] // "api/offers/<number>"
        public ActionResult<OfferDto> Get(int id)
        {
            // should return an offer having a specified id
            Offer offer = _offersService.GetById(id);
            return (offer != null) ? Ok(ToDto(offer)) : NotFound();
        }
        
        [HttpPost("add")]// "api/offers/add"
        public void PostOffer(Offer offer)
        {
            _offersService.Save(offer);
        }

        public OfferDto ToDto(Offer offer) => new()
        {
            Food = offer.Food,
            CreationDate = offer.CreationDate,
            Description = offer.Description,
            ExpirationDate = offer.ExpirationDate,
            Quantity = offer.Quantity,
            Giver = new GiverDto
            {
                Address = offer.Giver.Address,
                Name = offer.Giver.Name
            }
        };
    }
}