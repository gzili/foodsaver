using System.Collections.Generic;
using backend.Models;
using backend.Repositories;

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
    }
}