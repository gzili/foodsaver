using System;
using System.Collections.Generic;
using backend.Controllers;
using backend.Models;
using backend.Services;

namespace backend.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _appContext;

        public UserRepository()
        {
            _appContext = AppDbContext.GetObject();
        }

        public void Save(User user)
        {
            _appContext.Users.Add(user);
        }

        public List<User> GetAll()
        {
            return _appContext.Users;
        }

        public User GetById(int id)
        {
            return _appContext.GetById(_appContext.Users, id);
        }
    }
}