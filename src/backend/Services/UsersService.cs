using System.Linq;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace backend.Services
{
    public class UsersService : IUsersService
    {
        private readonly AppDbContext _db;

        private IQueryable<User> Users => _db.Users
            .Include(u => u.Address);

        public UsersService(AppDbContext db)
        {
            _db = db;
        }

        public void Create(User user)
        {
            user.Password = BC.HashPassword(user.Password);
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public User FindById(int id)
        {
            return Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetByEmail(string email)
        {
            return Users.FirstOrDefault(u => u.Email == email);
        }

        public User IsValidLogin(string email, string password)
        {
            var user = GetByEmail(email);
            return user != null && BC.Verify(password, user.Password) ? user : null;
        }
    }
}