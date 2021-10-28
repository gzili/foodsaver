using System;
using System.Collections.Generic;
using backend.Models;

namespace backend.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly AppDbContext _appContext;

        public UserRepository()
        {throw new NotImplementedException();
            /*_appContext = AppDbContext.GetObject();*/
        }

        public void Save(User user)
        {
            throw new NotImplementedException();
            /*_appContext.DbLists.Users.Add(user);*/
        }

        public List<User> GetAll()
        {
            throw new NotImplementedException();
            /*return _appContext.DbLists.Users;*/
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
            /*return _appContext.DbLists.Users.Find(x => x.Id == id);*/
        }

        public User GetByEmail(string email)
        {
            throw new NotImplementedException();
            /*return _appContext.DbLists.Users.Find(x => x.Email.Equals(email));*/
        }
    }
}