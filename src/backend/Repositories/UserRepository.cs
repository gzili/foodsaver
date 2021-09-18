using backend.Controllers;
using backend.Models;

namespace backend.Repositories
{
    public class UserRepository
    {
        public User GetUser()
        {
            return new User(1, "e@gmail.com", "edvinas", "pass", "vilnius", UserType.Individual);
        }
    }
}