using AutoMapper;
using backend.DTO.Reservation;
using backend.Extensions;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // /api/reservations
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationsService _reservationsService;
        private readonly IMapper _mapper;

        public ReservationsController(IReservationsService reservationsService, IMapper mapper)
        {
            _reservationsService = reservationsService;
            _mapper = mapper;
        }

        [HttpPost("{id:int}/completion")] // /api/reservations/{id}/completion
        public ActionResult<ReservationDto> Complete(int id, CompleteReservationDto dto)
        {
            var reservation = _reservationsService.FindById(id);
            var user = HttpContext.GetUser();

            if (reservation.Offer.Giver != user)
            {
                Log.Error(
                    "User {user.Id} tried to complete reservation {reservation.Id}",
                    user.Id,
                    reservation.Id);
                return Conflict("Reservation can only be marked as completed by the owner of the offer");
            }

            if (reservation.CompletedAt != null)
            {
                Log.Error(
                    "User {user.Id} Tried to complete an already completed reservation {reservation.Id}",
                    user.Id,
                    reservation.Id);
                return Conflict("Reservation is already completed");
            }

            if (reservation.Pin != dto.Pin)
            {
                return Conflict("Invalid PIN code");
            }
            
            _reservationsService.Complete(reservation);

            return _mapper.Map<ReservationDto>(reservation);
        }
    }
}