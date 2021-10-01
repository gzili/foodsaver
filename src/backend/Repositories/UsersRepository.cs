﻿using System;
using System.Collections.Generic;
using backend.Models;

namespace backend.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly AppDbContext _appContext;

        public UserRepository()
        {
            _appContext = AppDbContext.GetObject();
        }

        public void Save(User user)
        {
            _appContext.Users.Add(user);
            Console.WriteLine(string.Join("\n", _appContext.Users));
            Console.WriteLine();
        }

        public List<User> GetAll()
        {
            return _appContext.Users;
        }

        public User GetById(int id)
        {
            return _appContext.Users.Find(x => x.Id == id); 
        }

        public bool CheckLogin(User user)
        {
            return _appContext.Users.Contains(user);
        }

        public bool GetByEmail(string email)
        {
            return _appContext.Users.Find(x => x.Email.Equals(email)) == null;
        }
    }
}