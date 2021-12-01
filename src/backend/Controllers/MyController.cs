using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using backend.DTO.Offer;
using backend.DTO.User;
using backend.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // /api/my
    public class MyController : ControllerBase
    {
        private readonly IMapper _mapper;

        public MyController(IMapper mapper)
        {
            _mapper = mapper;
        }
        
        [Authorize]
        [HttpGet("profile")]
        public ActionResult<UserDto> Get() // GET /api/my/profile
        {
            var user = HttpContext.GetUser();
            
            return _mapper.Map<UserDto>(user);
        }
        
        [Authorize]
        [HttpGet("offers")] // GET /api/my/offers
        public IEnumerable<OfferDto> GetUserOffers()
        {
            var user = HttpContext.GetUser();
            
            return user.Offers.Select(_mapper.Map<OfferDto>).ToList();
        }
    }
}