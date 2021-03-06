using backend.DTO.Address;
using backend.Models;

namespace backend.DTO.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public UserType UserType { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public AddressDto Address { get; set; }
        public string AvatarPath { get; set; }
    }
}