using backend.Models;
using Microsoft.AspNetCore.Http;

namespace backend.DTO.User
{
    public class CreateUserDto
    {
        public UserType UserType { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string AddressStreet { get; set; }
        public string AddressCity { get; set; }
        public IFormFile Avatar { get; set; }
    }
}