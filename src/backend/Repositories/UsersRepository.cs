using System.Collections.Generic;
using System.Linq;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class UsersRepository
    {
        private readonly AppDbContext _db;

        public UsersRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Save(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public List<User> GetAll()
        {
            return _db.Users
                .Include(u => u.Address)
                .ToList();
        }

        public User GetById(int id)
        {
            return _db.Users
                .Include(u => u.Address)
                .FirstOrDefault(user => user.Id == id);
        }

        public User GetByEmail(string email)
        {
            return _db.Users
                .Include(u => u.Address)
                .FirstOrDefault(u => u.Email == email);
        }

        public List<Offer> GetOffersByUserId(int id)
        {
            var offers = _db.Users
                .Include(u => u.Address)
                .Include(u => u.Offers)
                .ThenInclude(o => o.Food)
                .Include(u => u.Offers)
                .ThenInclude(o => o.Giver)
                .Include(u => u.Offers)
                .ThenInclude(o => o.Address)
                .First(u => u.Id == id).Offers;
            return offers;
        }
    }
}