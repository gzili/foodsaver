using System.Collections.Generic;
using backend.Models;
using backend.Repositories;
using BC = BCrypt.Net.BCrypt;

namespace backend.Services
{
    public class UsersService
    {
        private readonly UsersRepository _usersRepository;

        public UsersService(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        
        public void Create(User user)
        {
            user.Password = BC.HashPassword(user.Password);
            _usersRepository.Save(user);
        }

        public User GetById(int id)
        {
            return _usersRepository.GetById(id);
        }

        public User GetByEmail(string email)
        {
            return _usersRepository.GetByEmail(email);
        }

        public IEnumerable<Offer> GetOffersByUserId(int id)
        {
            return _usersRepository.GetOffersByUserId(id);
        }

        public User IsValidLogin(string email, string password)
        {
            var user = _usersRepository.GetByEmail(email);
            return user != null && BC.Verify(password, user.Password) ? user : null;
        }
    }
}