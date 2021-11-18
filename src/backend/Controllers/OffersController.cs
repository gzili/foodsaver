using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using backend.Data;
using backend.DTO.Offers;
using backend.DTO.Reservation;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // "api/offers"
    public class OffersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly OffersService _offersService;
        private readonly PushService _pushService;
        private readonly Lazy<ReservationsService> _lazyReservationsService;
        private ReservationsService _reservationsService => _lazyReservationsService.Value;

        public OffersController(
            IMapper mapper,
            OffersService offersService,
            PushService pushService)
        {
            _mapper = mapper;
            _offersService = offersService;
            _pushService = pushService;
            _lazyReservationsService =
                new Lazy<ReservationsService>(() => HttpContext.RequestServices.GetService<ReservationsService>());
        }

        [HttpGet] // GET "api/offers"
        public IEnumerable<OfferDto> FindAll(bool showExpired)
        {
            return _offersService.FindAll(showExpired).Select(_mapper.Map<OfferDto>);
        }

        [HttpGet("{id:int}")] // GET "api/offers/<number>"
        public ActionResult<OfferDto> FindById(int id)
        {
            var offer = _offersService.FindById(id);
            return _mapper.Map<OfferDto>(offer);
        }

        [Authorize]
        [HttpPost] // POST "api/offers"
        public async Task<ActionResult<OfferDto>> Create([FromForm] CreateOfferDto createOfferDto)
        {
            var imagePath = await FileUploadService.UploadFormFileAsync(createOfferDto.FoodPhoto, "images");
            
            if (imagePath == null)
                return BadRequest("Invalid image file");
            
            var user = (User) HttpContext.Items["user"];
            
            var offer = _mapper.Map<Offer>(createOfferDto);
            offer.CreatedAt = DateTime.UtcNow;
            offer.Giver = user;
            offer.Food.ImagePath = imagePath;
            
            _offersService.Create(offer);
            
            _pushService.NotifyOffersChanged();

            return CreatedAtAction(nameof(FindById), new { id = offer.Id }, _mapper.Map<OfferDto>(offer));
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

            var user = (User) HttpContext.Items["user"];
            
            if(offer.Giver != user)
                return Conflict("Offer can only be updated by its owner");

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
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var offer = _offersService.FindById(id);

            _offersService.Delete(offer);
            
            _pushService.NotifyOfferDeleted(id);
            _pushService.NotifyOffersChanged();

            return NoContent();
        }

        [Authorize]
        [HttpPost("{id:int}/reservation")] // POST "api/offers/<number>/reservation
        public ActionResult<ReservationDto> CreateReservation(int id, CreateReservationDto createReservationDto)
        {
            var offer = _offersService.FindById(id);

            var user = (User) HttpContext.Items["user"];

            if (user == offer.Giver)
            {
                return Conflict("Offer cannot be reserved by its owner");
            }

            Reservation reservation;

            lock (ReservationsLock.Object)
            {
                reservation = offer.Reservations.FirstOrDefault(r => r.User == user);

                if (reservation != null)
                {
                    return Conflict("User already has an active reservation for this offer");
                }

                if (createReservationDto.Quantity > offer.AvailableQuantity)
                {
                    return Conflict("Requested quantity is larger than available");
                }

                reservation = new Reservation
                {
                    Offer = offer,
                    User = user,
                    CreatedAt = DateTime.Now,
                    Quantity = createReservationDto.Quantity
                };
            
                _reservationsService.Save(reservation);
            }

            return CreatedAtAction(
                nameof(GetReservation),
                new { id = reservation.Id },
                _mapper.Map<ReservationDto>(reservation));
        }

        [Authorize]
        [HttpGet("{id:int}/reservation")] // GET "api/offers/{id}/reservation
        public ActionResult<ReservationDto> GetReservation(int id)
        {
            var offer = _offersService.FindById(id);

            var user = (User) HttpContext.Items["user"];

            var reservation = offer.Reservations.FirstOrDefault(r => r.User == user);

            return _mapper.Map<ReservationDto>(reservation);
        }

        [Authorize]
        [HttpDelete("{id:int}/reservation")] // DELETE "api/offers/<number>/reservation
        public IActionResult CancelReservation(int id)
        {
            var offer = _offersService.FindById(id);

            var user = (User) HttpContext.Items["user"];

            lock (ReservationsLock.Object)
            {
                var reservation = offer.Reservations.FirstOrDefault(r => r.User == user);

                if (reservation == null)
                {
                    return Conflict("User has not reserved this offer");
                }
            
                _reservationsService.Delete(reservation);
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("{id:int}/reservations")]
        public ActionResult<IEnumerable<ReservationDto>> GetAllReservations(int id)
        {
            var offer = _offersService.FindById(id);
            
            var user = (User) HttpContext.Items["user"];

            if (offer.Giver != user)
            {
                return Conflict("Reservations can only be listed by the owner");
            }

            return Ok(offer.Reservations.Select(_mapper.Map<ReservationDto>));
        }
    }
}