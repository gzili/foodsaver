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

        private bool IsValidEmailRegistration(string email)
        {
            return _userRepository.GetByEmail(email) == null;
        }

        public bool IsValidRegister(CreateUserDto createUserDto)
        {
            return IsValidEmailRegistration(createUserDto.Email);
        }
        public string GetFirstValidationError(CreateUserDto createUserDto)
        {
            if (!ValidationService.IsEmail(createUserDto.Email))
                return "This email does not conform to our company standards";
            
            if (string.IsNullOrWhiteSpace(createUserDto.Password))
                return "Password must be non-blank";
            
            return null;
        }

        public User FromCreateDto(CreateUserDto createUserDto) => new()
        {
            // Real database will automatically assign a user id, this is temporary
            Id = GetAll().Count + 1,
            Email = createUserDto.Email,
            Name = createUserDto.Name,
            Password = BC.HashPassword(createUserDto.Password),
            Address = createUserDto.Address,
            UserType = createUserDto.UserType
        };
    }
}