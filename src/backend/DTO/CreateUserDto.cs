using backend.Models;

namespace backend.DTO
{
    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Address Address { get; set; } 
        public UserType UserType { get; set; }
    }
}