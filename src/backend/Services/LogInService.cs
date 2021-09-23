﻿using System.Collections.Generic;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class LogInService
    {
        private readonly UserRepository _userRepository;

        public LogInService()
        {
            _userRepository = new UserRepository();
        }

        public User GetUser()
        {
            return UserRepository.GetUser();
        }

        public void Save(User user)
        {
            UserRepository.Save(user);
        }

        public List<User> GetAll()
        {
            return UserRepository.GetAll();
        }
    }
}