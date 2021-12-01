using System;
using backend.DTO.User;

namespace backend.DTO.Reservation
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public decimal Quantity { get; set; }
        public UserDto User { get; set; }
    }
}