using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using backend.DTO;
using backend.DTO.Offer;
using backend.DTO.Reservation;
using backend.Exceptions;
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
        private readonly IOffersService _offersService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IFileService _fileService;
        private readonly IReservationsService _reservationsService;

        public OffersController(
            IOffersService offersService,
            IMapper mapper,
            IConfiguration config,
            IFileService fileService,
            IReservationsService reservationsService)
        {
            _offersService = offersService;
            _mapper = mapper;
            _config = config;
            _fileService = fileService;
            _reservationsService = reservationsService;
        }

        [HttpGet] // GET /api/offers
        public ActionResult<PaginatedListDto<OfferDto>> FindAll(bool showExpired, int page, int limit)
        {
            var paginatedOffersList = _offersService.FindAllPaginated(showExpired, page, limit);
            var paginatedOffersListDto = new PaginatedListDto<OfferDto>
            {
                Data = paginatedOffersList.Select(_mapper.Map<OfferDto>).ToList(),
                HasNextPage = paginatedOffersList.HasNextPage
            };
            return paginatedOffersListDto;
        }

        [HttpGet("{id:int}")] // GET /api/offers/{id}
        public OfferDto FindById(int id)
        {
            var offer = _offersService.FindById(id);
            return _mapper.Map<OfferDto>(offer);
        }

        [Authorize]
        [HttpPost] // POST /api/offers
        public async Task<ActionResult<OfferDto>> Create([FromForm] CreateOfferDto createOfferDto)
        {
            var imagePath = await _fileService.UploadFormFileAsync(createOfferDto.FoodPhoto, _config["UploadedFilesPath"]);
            
            if (imagePath == null)
                return BadRequest("Invalid image file");
            
            var user = HttpContext.GetUser();
            
            var offer = _mapper.Map<Offer>(createOfferDto);
            offer.AvailableQuantity = offer.Quantity;
            offer.CreatedAt = DateTime.UtcNow;
            offer.Food.ImagePath = imagePath;
            offer.Giver = user;

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

            var imagePath = await _fileService.UploadFormFileAsync(image, _config["UploadedFilesPath"]);

            // if a new file was uploaded, swap the new path with the old one
            if (imagePath != null)
                (offer.Food.ImagePath, imagePath) = (imagePath, offer.Food.ImagePath);

            _offersService.Update(offer, updateOfferDto, foodDto);
            
            // delete the old file if changes were saved successfully
            if (imagePath != null)
                _fileService.DeleteFile(imagePath);

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
            var offer = _offersService.FindById(id);
            var user = HttpContext.GetUser();

            if (user == offer.Giver)
            {
                return Conflict("Offer cannot be reserved by its owner");
            }
            
            var reservation = offer.Reservations.FirstOrDefault(r => r.User == user);

            if (reservation != null)
            {
                return Conflict("User already has an active reservation for this offer");
            }
            
            reservation = new Reservation
            {
                Offer = offer,
                User = user,
                Quantity = createReservationDto.Quantity
            };
            
            try
            {
                _reservationsService.Create(reservation);
            }
            catch (QuantityTooLargeException)
            {
                return Conflict("Requested quantity is larger than available");
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