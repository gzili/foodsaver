using backend.DTO.Address;
using backend.Models;

namespace backend.DTO.User
{
    public class PlaceDto
    {
        public int Id { get; set; }
        public int ActiveOffersCount { get; set; }
        public int CompletedReservationsCount { get; set; }
        public AddressDto Address { get; set; }
        public string AvatarPath { get; set; }
        public string Name { get; set; }
        public UserType Type { get; set; }
    }
}