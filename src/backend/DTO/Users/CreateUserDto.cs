using backend.Models;
using Microsoft.AspNetCore.Http;

namespace backend.DTO.Users
{
    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public IFormFile Avatar { get; set; }
        public string Password { get; set; }
        public Address Address { get; set; }
        public UserType UserType { get; set; }
    }
}