using System;
using System.Collections.Generic;
using backend.Controllers;
using backend.Models;

namespace backend.Repositories
{
    public class UserRepository
    {
        private static List<User> Users { get; set; } = new();

        public static User Get()
        {
            return new User(new Random().Next(), "e@gmail.com", "edvinas", "pass", new Address { City = "Vilnius", Country = "Lithuania", State = "NO STATE", PostalCode = 89974, AddressLine1 = "Kroniku street 9999", AddressLine2 = "-80 aukstas 3 butas"}, UserType.Individual);
        }

        public static void Save(User user)
        {
            Users.Add(user);
        }

        public static List<User> GetAll()
        {
            return Users;
        }
    }
}