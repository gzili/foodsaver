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
            return new User(new Random().Next(), "e@gmail.com", "edvinas", "pass", "vilnius", UserType.Individual);
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