using System.Collections.Generic;
using backend.DTO.Offers;
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

        private bool CheckEmailForRegistration(string email)
        {
            return _userRepository.GetByEmail(email) == null;
        }

        public bool CheckRegister(CreateUserDto createUserDto)
        {
            return CheckEmailForRegistration(createUserDto.Email);
        }
        public bool Validate(CreateUserDto createUserDto)
        {
            return ValidationService.validateUser(createUserDto);
        }
        
        public User FromCreateDto(CreateUserDto createUserDto)
        {
            return new User(
                //TODO this is bad practice, because realistically it should query all users, which would add a lot of work on DB
                //TODO we should find something similar to JPA IDENTITY 
                GetAll().Count + 1,
                createUserDto.Email,
                createUserDto.Name,
                BC.HashPassword(createUserDto.Password),
                createUserDto.Address,
                createUserDto.UserType
            );
        }
    }
}