using System.Linq;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class UsersRepository : IRepositoryBase<User>
    {
        private readonly AppDbContext _db;

        public IQueryable<User> Items => _db.Users.Include(u => u.Address);

        public UsersRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Create(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }
    }
}