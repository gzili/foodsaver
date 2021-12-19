using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using backend.DTO.Offer;
using backend.DTO.User;
using backend.Extensions;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // /api/my
    public class MyController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOffersService _offersService;
        private readonly IReservationsService _reservationsService;

        public MyController(IMapper mapper, IOffersService offersService, IReservationsService reservationsService)
        {
            _mapper = mapper;
            _offersService = offersService;
            _reservationsService = reservationsService;
        }
        
        [Authorize]
        [HttpGet("profile")]
        public ActionResult<UserDto> GetProfile() // GET /api/my/profile
        {
            var user = HttpContext.GetUser();
            
            return _mapper.Map<UserDto>(user);
        }
        
        [Authorize]
        [HttpGet("offers")] // GET /api/my/offers
        public IEnumerable<OfferDto> GetOffers()
        {
            var user = HttpContext.GetUser();
            return _offersService.FindAllByUserId(user.Id).Select(_mapper.Map<OfferDto>);
        }

        [Authorize]
        [HttpGet("reservations")]
        public IEnumerable<OfferDto> GetReservations()
        {
            var user = HttpContext.GetUser();
            var reservations = _reservationsService.GetReservedOffersByUserId(user.Id);
            return reservations.Select(_mapper.Map<OfferDto>);
        }
    }
}