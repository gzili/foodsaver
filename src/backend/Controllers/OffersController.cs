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
        private readonly UserService _userService;

        public OffersController(FileUploadService fileUploadService, OffersService offersService, UserService userService)
        {
            _fileUploadService = fileUploadService;
            _offersService = offersService;
            _userService = userService;
        }

        [HttpGet] // "api/offers"
        public IEnumerable<OfferDto> FindAll()
        {
            return _offersService.GetAll().Select(_offersService.ToDto);
        }

        [HttpGet("{id:int}")] // "api/offers/<number>"
        public ActionResult<OfferDto> FindById(int id)
        {
            var offer = _offersService.GetById(id);
            return offer != null ? Ok(_offersService.ToDto(offer)) : NotFound();
        }

        [Authorize]
        [HttpPost] // "api/offers"
        public async Task<ActionResult<OfferCreateDto>> Create([FromForm] CreateOfferDto createOfferDto)
        {
            if (createOfferDto.ExpirationDate < DateTime.Now)
                return BadRequest("Invalid expiration date");

            var path = await _fileUploadService.UploadFormFileAsync(createOfferDto.FoodPhoto, "images");
            
            if (path == null)
                return BadRequest("Invalid image file");
            
            var offerCreateDto = new OfferCreateDto
            {
                FoodName = createOfferDto.FoodName,
                FoodUnit = createOfferDto.FoodUnit,
                Quantity = createOfferDto.Quantity,
                ExpirationDate = createOfferDto.ExpirationDate,
                Description = createOfferDto.Description,
                FoodImagePath = path
            };
            var userId = int.Parse(HttpContext.User.Identity.Name);
            _offersService.SaveDto(user: _userService.GetById(userId), offerCreateDto: offerCreateDto);
            
            return Ok();
        }
    }
}