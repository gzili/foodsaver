using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTO.Offers;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // "api/offers"
    public class OffersController : ControllerBase
    {
        private readonly FileUploadService _fileUploadService;
        private readonly OffersService _offersService;
        private readonly UsersService _usersService;
        
        public OffersController(FileUploadService fileUploadService, OffersService offersService, UsersService usersService)
        {
            _fileUploadService = fileUploadService;
            _offersService = offersService;
            _usersService = usersService;
        }

        [HttpGet] // "api/offers"
        public IEnumerable<OfferDto> FindAll()
        {
            var offers = Request.Query.ContainsKey("showExpired")
                ? _offersService.GetAll()
                : _offersService.GetAllActiveOffers();
            return offers.Select(_offersService.ToDto);
        }

        [HttpGet("{id:int}")] // "api/offers/<number>"
        public ActionResult<OfferDto> FindById(int id)
        {
            var offer = _offersService.GetById(id);
            return offer != null ? Ok(_offersService.ToDto(offer)) : NotFound();
        }

        [Authorize]
        [HttpPost] // "api/offers"
        public async Task<ActionResult<CreateOfferDto>> Create([FromForm] CreateOfferDto createOfferDto)
        {
            if (createOfferDto.ExpirationDate < DateTime.Now)
                return BadRequest("Invalid expiration date");

            var path = await _fileUploadService.UploadFormFileAsync(createOfferDto.FoodPhoto, "images");
            
            if (path == null)
                return BadRequest("Invalid image file");
            
            var userId = int.Parse(HttpContext.User.Identity.Name);
            var offer = _offersService.SaveDto(createOfferDto, path, _usersService.GetById(userId));

            return Ok(_offersService.ToDto(offer));
        }
    }
}