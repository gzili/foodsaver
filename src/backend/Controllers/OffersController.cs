using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using backend.Data;
using backend.DTO.Offer;
using backend.DTO.Reservation;
using backend.Extensions;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // /api/offers
    public class OffersController : ControllerBase
    {
        private readonly FileUploadService _fileUploadService;
        private readonly IMapper _mapper;
        private readonly OffersService _offersService;
        private readonly ReservationsService _reservationsService;

        private readonly string _uploadPath;

        public OffersController(
            FileUploadService fileUploadService,
            IConfiguration config,
            IMapper mapper,
            OffersService offersService,
            ReservationsService reservationsService)
        {
            _fileUploadService = fileUploadService;
            _mapper = mapper;
            _uploadPath = config["UploadedFilesPath"];
            _offersService = offersService;
            _reservationsService = reservationsService;
        }

        [HttpGet] // GET /api/offers
        public IEnumerable<OfferDto> FindAll(bool showExpired)
        {
            return _offersService.FindAll(showExpired).Select(_mapper.Map<OfferDto>);
        }

        [HttpGet("{id:int}")] // GET /api/offers/{id}
        public ActionResult<OfferDto> FindById(int id)
        {
            var offer = _offersService.FindById(id);
            return _mapper.Map<OfferDto>(offer);
        }

        [Authorize]
        [HttpPost] // POST /api/offers
        public async Task<ActionResult<OfferDto>> Create([FromForm] CreateOfferDto createOfferDto)
        {
            var imagePath = await _fileUploadService.UploadFormFileAsync(createOfferDto.FoodPhoto, _uploadPath);
            
            if (imagePath == null)
                return BadRequest("Invalid image file");
            
            var user = HttpContext.GetUser();
            
            var offer = _mapper.Map<Offer>(createOfferDto);
            offer.CreatedAt = DateTime.UtcNow;
            offer.Giver = user;
            offer.Food.ImagePath = imagePath;
            
            _offersService.Create(offer);

            return CreatedAtAction(nameof(FindById), new { id = offer.Id }, _mapper.Map<OfferDto>(offer));
        }
        
        [Authorize]
        [HttpPut("{id:int}")] // PUT /api/offers/{id}
        public async Task<ActionResult<OfferDto>> Update(
            int id,
            [FromForm] UpdateOfferDto updateOfferDto,
            [FromForm] FoodDto foodDto,
            IFormFile image)
        {
            var offer = _offersService.FindById(id);
            var user = HttpContext.GetUser();
            
            if(offer.Giver != user)
                return Conflict("Offer can only be updated by its owner");

            var imagePath = await _fileUploadService.UploadFormFileAsync(image, _uploadPath);

            // if a new file was uploaded, swap the new path with the old one
            if (imagePath != null)
                (offer.Food.ImagePath, imagePath) = (imagePath, offer.Food.ImagePath);

            _offersService.Update(offer, updateOfferDto, foodDto);
            
            // delete the old file if changes were saved successfully
            if (imagePath != null)
                _fileUploadService.DeleteFile(imagePath);

            return _mapper.Map<OfferDto>(offer);
        }

        [Authorize]
        [HttpDelete("{id:int}")] // DELETE /api/offers/{id}
        public IActionResult Delete(int id)
        {
            var offer = _offersService.FindById(id);

            _offersService.Delete(offer);

            return NoContent();
        }

        [Authorize]
        [HttpPost("{id:int}/reservation")] // POST /api/offers/{id}/reservation
        public ActionResult<ReservationDto> CreateReservation(int id, CreateReservationDto createReservationDto)
        {
            var user = HttpContext.GetUser();
            Reservation reservation;

            lock (ReservationsLock.Object)
            {
                var offer = _offersService.FindById(id);

                if (user == offer.Giver)
                {
                    return Conflict("Offer cannot be reserved by its owner");
                }
                
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
                    CreatedAt = DateTime.UtcNow,
                    Quantity = createReservationDto.Quantity
                };
            
                _reservationsService.Create(reservation);
            }

            return CreatedAtAction(
                nameof(GetReservation),
                new { id = reservation.Id },
                _mapper.Map<ReservationDto>(reservation));
        }

        [Authorize]
        [HttpGet("{id:int}/reservation")] // GET /api/offers/{id}/reservation
        public ActionResult<ReservationDto> GetReservation(int id)
        {
            var offer = _offersService.FindById(id);
            var user = HttpContext.GetUser();

            var reservation = offer.Reservations.FirstOrDefault(r => r.User == user);

            return _mapper.Map<ReservationDto>(reservation);
        }

        [Authorize]
        [HttpDelete("{id:int}/reservation")] // DELETE /api/offers/{id}/reservation
        public IActionResult CancelReservation(int id)
        {
            var offer = _offersService.FindById(id);
            var user = HttpContext.GetUser();
            
            var reservation = offer.Reservations.FirstOrDefault(r => r.User == user);

            if (reservation == null)
            {
                return Conflict("User has not reserved this offer");
            }

            if (reservation.CompletedAt != null)
            {
                return Conflict("Completed reservation can not be cancelled");
            }
        
            _reservationsService.Delete(reservation);

            return Ok();
        }

        [Authorize]
        [HttpGet("{id:int}/reservations")] // GET /api/offers/{id}/reservations
        public ActionResult<IEnumerable<ReservationDto>> GetAllReservations(int id)
        {
            var offer = _offersService.FindById(id);
            var user = HttpContext.GetUser();

            if (offer.Giver != user)
            {
                return Conflict("Reservations can only be listed by the owner");
            }

            return Ok(offer.Reservations.Select(_mapper.Map<ReservationDto>));
        }
    }
}