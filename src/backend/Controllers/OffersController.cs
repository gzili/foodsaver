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
using Microsoft.AspNetCore.Http;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // "api/offers"
    public class OffersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly OffersService _offersService;

        public OffersController(IMapper mapper, OffersService offersService)
        {
            _mapper = mapper;
            _offersService = offersService;
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

            var imagePath = await FileUploadService.UploadFormFileAsync(createOfferDto.FoodPhoto, "images");
            
            if (imagePath == null)
                return BadRequest("Invalid image file");
            
            var user = (User) HttpContext.Items["user"];
            
            var offer = _mapper.Map<Offer>(createOfferDto);
            offer.CreatedAt = DateTime.Now;
            offer.Giver = user;
            offer.Food.ImagePath = imagePath;
            
            _offersService.Save(offer);

            return Ok(_mapper.Map<OfferDto>(offer));
        }
        
        
        [Authorize]
        [HttpPut("{id:int}")] // "api/offers/{id}" id of the offer
        public async Task<ActionResult<OfferDto>> Update(
            int id,
            [FromForm] UpdateOfferDto updateOfferDto,
            [FromForm] FoodDto foodDto,
            IFormFile image)
        {
            var offer = _offersService.GetById(id);

            if (offer == null)
                return NotFound();
            
            var user = (User) HttpContext.Items["user"];
            
            if(offer.Giver != user)
                return BadRequest("Offer can only be updated by its owner");

            var imagePath = await FileUploadService.UploadFormFileAsync(image, "images");

            // if a new file was uploaded, swap the new path with the old one
            if (imagePath != null)
                (offer.Food.ImagePath, imagePath) = (imagePath, offer.Food.ImagePath);

            _offersService.UpdateOffer(offer, updateOfferDto, foodDto);
            
            // delete the old file if changes were saved successfully
            if (imagePath != null)
                FileUploadService.DeleteFile(imagePath);

            return _mapper.Map<OfferDto>(offer);
        }
    }
}