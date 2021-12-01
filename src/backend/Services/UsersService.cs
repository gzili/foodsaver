using System.Linq;
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
            _usersRepository.Create(user);
        }

        public User GetByEmail(string email)
        {
            return _usersRepository.Items.FirstOrDefault(u => u.Email == email);
        }

        public User IsValidLogin(string email, string password)
        {
            var user = GetByEmail(email);
            return user != null && BC.Verify(password, user.Password) ? user : null;
        }
    }
}