using System.Collections.Generic;
using System.Linq;
using backend.Data;
using backend.Models;

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
            return _db.Users.ToList();
        }

        public User GetById(int id)
        {
            return _db.Users.Find(id);
        }

        public User GetByEmail(string email)
        {
            return _db.Users.FirstOrDefault(user => user.Email == email);
        }

        public List<Offer> GetOffersByUserId(int id)
        {
            return _db.Users.Find(id).Offers.ToList();
        }
    }
}