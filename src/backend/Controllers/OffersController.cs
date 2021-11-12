using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using backend.DTO.Offers;
using backend.DTO.Reservation;
using backend.Exceptions;
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
        private readonly ReservationsService _reservationsService;

        public OffersController(
            IMapper mapper,
            OffersService offersService,
            ReservationsService reservationsService)
        {
            _mapper = mapper;
            _offersService = offersService;
            _reservationsService = reservationsService;
        }

        [HttpGet] // GET "api/offers"
        public IEnumerable<OfferDto> FindAll()
        {
            var offers = Request.Query.ContainsKey("showExpired")
                ? _offersService.FindAll()
                : _offersService.FindAllActiveOffers();
            return offers.Select(_mapper.Map<OfferDto>);
        }

        [HttpGet("{id:int}")] // GET "api/offers/<number>"
        public ActionResult<OfferDto> FindById(int id)
        {
            try
            {
                var offer = _offersService.FindById(id);
                return _mapper.Map<OfferDto>(offer);
            }
            catch (NoSuchOfferException e)
            {
                return NoContent();
            }
        }

        [Authorize]
        [HttpPost] // POST "api/offers"
        public async Task<ActionResult<OfferDto>> Create([FromForm] CreateOfferDto createOfferDto)
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

            _offersService.Create(offer);

            return _mapper.Map<OfferDto>(offer);
        }

        [Authorize]
        [HttpPut("{id:int}")] // "api/offers/{id}" id of the offer
        public async Task<ActionResult<OfferDto>> Update(
            int id,
            [FromForm] UpdateOfferDto updateOfferDto,
            [FromForm] FoodDto foodDto,
            IFormFile image)
        {
            var offer = _offersService.FindById(id);

            if (offer == null)
                return NotFound();

            var user = (User) HttpContext.Items["user"];

            if (offer.Giver != user)
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

        [Authorize]
        [HttpPost("{id:int}/reservations")] // POST "api/offers/<number>/reservations
        public IActionResult CreateReservation(int id, ReservationDto reservationDto)
        {
            var offer = _offersService.FindById(id);

            if (offer == null)
            {
                return BadRequest("Invalid offer ID");
            }

            var user = (User) HttpContext.Items["user"];

            var reservation = offer.Reservations.FirstOrDefault(r => r.User == user);

            if (reservation != null)
            {
                return BadRequest("User already has an active reservation for this offer");
            }

            var availableQuantity = offer.Quantity - offer.ReservedQuantity;
            var requestedQuantity = reservationDto.Quantity;

            if (requestedQuantity > availableQuantity)
            {
                return BadRequest("Requested quantity is larger than available");
            }

            reservation = new Reservation
            {
                Offer = offer,
                User = user,
                CreatedAt = DateTime.Now,
                Quantity = requestedQuantity
            };

            _reservationsService.Save(reservation);

            return Ok();
        }

        [Authorize]
        [HttpDelete("{id:int}/reservations")] // DELETE "api/offers/<number>/reservations
        public IActionResult CancelReservation(int id)
        {
            var offer = _offersService.FindById(id);

            if (offer == null)
            {
                return BadRequest("Invalid offer ID");
            }

            var user = (User) HttpContext.Items["user"];

            var reservation = offer.Reservations.FirstOrDefault(r => r.User == user);

            if (reservation == null)
            {
                return BadRequest("User has not reserved this offer");
            }

            _reservationsService.Delete(reservation);

            return Ok();
        }
    }
}