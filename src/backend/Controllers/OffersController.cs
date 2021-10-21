using System;
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

        private readonly UserService _userService;

        public OffersController(OffersService offersService, UserService userService)
        {
            _offersService = offersService;
            _userService = userService;
        }

        [HttpGet] // "api/offers"
        public IEnumerable<OfferDto> FindAllActive()
        {
            return _offersService.GetAllActive().Select(_offersService.ToDto);
        }
        
        [HttpGet("{showExpired:bool}")] // "api/offers/<showExpired>"
        public IEnumerable<OfferDto> FindAll(bool showExpired)
        {
            return showExpired ? _offersService.GetAll().Select(_offersService.ToDto) : _offersService.GetAllActive().Select(_offersService.ToDto);
        }

        [HttpGet("{id:int}")] // "api/offers/<number>"
        public ActionResult<OfferDto> FindById(int id)
        {
            var offer = _offersService.GetById(id);
            return offer != null ? Ok(_offersService.ToDto(offer)) : NotFound();
        }

        [Authorize]
        [HttpPost("add")] // "api/offers/add"
        public ActionResult<OfferCreateDto> Create(OfferCreateDto offerCreateDto)
        {
            if (offerCreateDto.ExpirationDate < DateTime.Now)
                return Conflict(offerCreateDto);
            var userId = int.Parse(HttpContext.User.Identity.Name);
            _offersService.SaveDto(user: _userService.GetById(userId), offerCreateDto: offerCreateDto);
            return Ok();
        }

        [HttpGet("byUser")]
        public Dictionary<int, IEnumerable<Offer>> FindGrouped()
        {
            return _offersService.GetGrouped();
        }
    }
}