using System;
using System.Linq;
using System.Linq.Expressions;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class UsersRepository: IRepositoryBase<User>
    {
        private readonly AppDbContext _db;

        public UsersRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Create(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public IQueryable<User> FindAll()
        {
            return _db.Users.Include(u => u.Address);
        }

        public IQueryable<User> FindByCondition(Expression<Func<User, bool>> expression)
        {
            return FindAll().Where(expression);
        }
    }
}