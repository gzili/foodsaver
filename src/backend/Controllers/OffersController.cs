using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly FileUploadService _fileUploadService;
        private readonly IMapper _mapper;
        private readonly OffersService _offersService;
        private readonly UsersService _usersService;
        
        public OffersController(
            FileUploadService fileUploadService,
            IMapper mapper,
            OffersService offersService,
            UsersService usersService)
        {
            _fileUploadService = fileUploadService;
            _mapper = mapper;
            _offersService = offersService;
            _usersService = usersService;
        }

        [HttpGet] // "api/offers"
        public IEnumerable<OfferDto> FindAll()
        {
            var offers = Request.Query.ContainsKey("showExpired")
                ? _offersService.GetAll()
                : _offersService.GetAllActiveOffers();
            return offers.Select(_mapper.Map<OfferDto>);
        }

        [HttpGet("{id:int}")] // "api/offers/<number>"
        public ActionResult<OfferDto> FindById(int id)
        {
            var offer = _offersService.GetById(id);
            return offer != null ? Ok(_mapper.Map<OfferDto>(offer)) : NotFound();
        }

        [Authorize]
        [HttpPost] // "api/offers"
        public async Task<ActionResult<CreateOfferDto>> Create([FromForm] CreateOfferDto createOfferDto)
        {
            if (createOfferDto.ExpiresAt < DateTime.Now)
                return BadRequest("Invalid expiration date");

            var imagePath = await _fileUploadService.UploadFormFileAsync(createOfferDto.FoodPhoto, "images");
            
            if (imagePath == null)
                return BadRequest("Invalid image file");
            
            var userId = int.Parse(HttpContext.User.Identity.Name);
            var user = _usersService.GetById(userId);
            
            var offer = _mapper.Map<Offer>(createOfferDto);
            offer.CreatedAt = DateTime.Now;
            offer.Giver = user;
            offer.Food.ImagePath = imagePath;
            
            _offersService.Save(offer);

            return Ok(_mapper.Map<OfferDto>(offer));
        }
    }
}