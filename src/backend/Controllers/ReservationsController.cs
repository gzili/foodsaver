using AutoMapper;
using backend.DTO.Reservation;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ReservationsService _reservationsService;

        public ReservationsController(IMapper mapper, ReservationsService reservationsService)
        {
            _reservationsService = reservationsService;
            _mapper = mapper;
        }

        [HttpPost("{id:int}/completion")]
        public ActionResult<ReservationDto> Complete(int id)
        {
            var reservation = _reservationsService.FindById(id);
            var user = (User) HttpContext.Items["user"];

            if (reservation.Offer.Giver != user)
            {
                return Conflict("Reservation can only be marked as completed by the owner of the offer");
            }
            
            _reservationsService.Complete(reservation);

            return _mapper.Map<ReservationDto>(reservation);
        }
    }
}