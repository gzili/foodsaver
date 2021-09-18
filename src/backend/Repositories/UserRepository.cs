using System.Collections.Generic;
using backend.Controllers;
using backend.Models;

namespace backend.Repositories
{
    public class UserRepository
    {
        private List<User> Users { get; set; }

        public UserRepository()
        {
            Users = new List<User>();
        }

        public User GetUser()
        {
            return new User(1, "e@gmail.com", "edvinas", "pass", "vilnius", UserType.Individual);
        }

        public void Save(User user)
        {
            Users.Add(user);
        }
    }
}