using backend.DTO.Address;
using backend.Models;

namespace backend.DTO.Offer
{
    public class GiverDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public UserType UserType { get; set; }
        public string AvatarPath { get; set; }
        public AddressDto Address { get; set; }
    }
}