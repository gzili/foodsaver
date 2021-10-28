using System.Collections.Generic;
using backend.DTO.Users;
using backend.Models;
using backend.Repositories;
using BC = BCrypt.Net.BCrypt;

namespace backend.Services
{
    public class UserService
    {
        private readonly UsersRepository _usersRepository;

        public UserService(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public User GetById(int id)
        {
            return _usersRepository.GetById(id);
        }

        public List<User> GetAll()
        {
            return _usersRepository.GetAll();
        }

        public List<Offer> GetOffersByUserId(int id)
        {
            return _usersRepository.GetOffersByUserId(id);
        }

        public void Save(User user)
        {
            _usersRepository.Save(user);
        }

        public User CheckLogin(LoginUserDto dto)
        {
            var user = _usersRepository.GetByEmail(dto.Email);
            return user != null && BC.Verify(user.Password, user.Password) ? user : null;
        }

        private bool IsValidEmailRegistration(string email)
        {
            return _usersRepository.GetByEmail(email) == null;
        }

        public bool IsValidRegister(CreateUserDto createUserDto)
        {
            return IsValidEmailRegistration(createUserDto.Email);
        }

        public User FromCreateDto(CreateUserDto createUserDto) => new()
        {
            Email = createUserDto.Email,
            Username = createUserDto.Name,
            Password = BC.HashPassword(createUserDto.Password),
            Address = createUserDto.Address,
            UserType = createUserDto.UserType
        };
    }
}