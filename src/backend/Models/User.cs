using System;
using backend.Controllers;

namespace backend.Models
{
    public class User : ModelClass
    {
        public User(int id, string email, string name, string password, string location, UserType userType) : base(id)
        {
            Email = email;
            Name = name;
            Password = password;
            Location = location;
            UserType = userType;
        }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Location { get; set; } 
        public UserType UserType { get; set; }

        public override string ToString()
        {
            return Id + ", " + Email + ", " + Name + ", " + Password + ", " + Location + ", " + UserType;
        }
    }
}