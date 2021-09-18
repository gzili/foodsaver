using System;
using backend.Controllers;

namespace backend.Models
{
    public class User
    {
        public User(int id, string email, string name, string password, string location, UserType userType)
        {
            Id = id;
            Email = email;
            Name = name;
            Password = password;
            Location = location;
            UserType = userType;
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Location { get; set; } 
        public UserType UserType { get; set; }

        public string ToString()
        {
            return Id + ", " + Email + ", " + Name + ", " + Password + ", " + Location + ", " + UserType;
        }
    }
}