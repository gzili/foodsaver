using System.Collections.Generic;
using backend.DTO.Users;
using backend.Models;
using backend.Repositories;
using BC = BCrypt.Net.BCrypt;

namespace backend.Services
{
    public class UserService : IService<User>
    {
        private readonly UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }
        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public List<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public void Save(User user)
        {
            _userRepository.Save(user);
        }

        public User CheckLogin(LoginUserDto user)
        {
            var dbUser = _userRepository.GetByEmail(user.Email);
            return dbUser != null && BC.Verify(user.Password, dbUser.Password) ? dbUser : null;
        }

        public bool EmailRegistered(string email)
        {
            return EmailRegisteredInner(email);
        }

        private bool EmailRegisteredInner(string email)
        {
            return _userRepository.GetByEmail(email) != null;
        }

        public bool CheckRegister(User user)
        {
            return CheckRegisterInner(user);
        }

        private bool CheckRegisterInner(User user)
        {
            return _userRepository.GetByEmail(user.Email) == null;
        }
    }
}