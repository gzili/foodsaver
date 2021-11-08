using backend.DTO.Address;
using backend.Models;
using Microsoft.AspNetCore.Http;

namespace backend.DTO.Users
{
    public class CreateUserDto
    {
        public UserType UserType { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public AddressDto Address { get; set; }
        public IFormFile Avatar { get; set; }
    }
}