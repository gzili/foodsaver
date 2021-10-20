using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        [HttpPost("add")] // "api/offers/add"
        public async Task<ActionResult<OfferCreateDto>> Create([FromForm] CreateOfferDto createOfferDto)
        {
            if (createOfferDto.ExpirationDate < DateTime.Now)
                return Conflict(createOfferDto);
            var file = createOfferDto.FoodPhoto;
            if (file == null)
                return BadRequest("No photo provided");
            if (file.Length == 0)
                return BadRequest("Upload of a directory is not allowed");
            var path = Path.Combine("images", Path.GetRandomFileName() + Path.GetExtension(file.FileName));
            var fullPath = Path.Combine("wwwroot", path);
            await using (var stream = System.IO.File.Create(fullPath))
            {
                await file.CopyToAsync(stream);
            }
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

        [HttpGet("byUser")]
        public Dictionary<int, IEnumerable<Offer>> FindGrouped()
        {
            return _offersService.GetGrouped();
        }
    }
}