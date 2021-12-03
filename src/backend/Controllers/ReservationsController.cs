using AutoMapper;
using backend.DTO.Reservation;
using backend.Extensions;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<ReservationDto> Complete(int id)
        {
            var reservation = _reservationsService.FindById(id);
            var user = HttpContext.GetUser();

            if (reservation.Offer.Giver != user)
            {
                return Conflict("Reservation can only be marked as completed by the owner of the offer");
            }

            if (reservation.CompletedAt != null)
            {
                return Conflict("Reservation is already completed");
            }
            
            _reservationsService.Complete(reservation);

            return _mapper.Map<ReservationDto>(reservation);
        }
    }
}