﻿using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class RegisterService
    {
        private readonly UserRepository _userRepository;

        public RegisterService()
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
    }
}