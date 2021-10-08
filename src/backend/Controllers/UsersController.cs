using System.Collections.Generic;
using backend.DTO;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BC = BCrypt.Net.BCrypt;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserService _userService;

        public UsersController()
        {
            _userService = new UserService();
        }

        // Debug method, must be deleted Later
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _userService.GetAll();
        }

        /*
        [HttpGet("{id:int}")] // "api/users/<number>"
        public User Get(int id)
        {
            return _userService.GetById(id);
        }*/

        [HttpPost("login")] // "api/users/login"
        public ActionResult<User> Login(User newUser)
        {
            User user = _userService.CheckLogin(newUser);
            return user == null ? Conflict() : Ok(user);
        }

        [HttpPost("register")] // "api/users/register"
        public ActionResult<User> Register(CreateUserDto createUserDto)
        {
            HttpContext.SignOutAsync();
            User user = FromCreateDto(createUserDto);
            if (!_userService.CheckRegister(user)) return Conflict();
            _userService.Save(user);
            return Ok(user);
        }

        public User FromCreateDto(CreateUserDto createUserDto) => new(
            _userService.GetAll().Count + 1, // why always 0?
            createUserDto.Email,
            createUserDto.Name,
            BC.HashPassword(createUserDto.Password),
            createUserDto.Address,
            createUserDto.UserType
            );
    }
}