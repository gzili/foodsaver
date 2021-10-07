using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using backend.DTO;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        [HttpGet]
        public ActionResult<User> Get() // "api/users"
        {
            int currentUserId = getCurrentUserId();
            User user = _userService.GetById(currentUserId);
            return Ok(user);
        }

        /*
        [HttpGet("{id:int}")] // "api/users/<number>"
        public User Get(int id)
        {
            return _userService.GetById(id);
        }*/

        [HttpPost("login")] // "api/users/login"
        public ActionResult<User> Login(LoginUserDto loginUserDto)
        {
            User user = _userService.CheckLogin(loginUserDto);
            if (user == null) return Conflict();
            var claims = new List<Claim> { new Claim("id", user.Id.ToString()) };
            HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, "id", "")));
            return NoContent();
        }

        [HttpPost("logout")]
        public ActionResult LogOut()
        {
            HttpContext.SignOutAsync();
            return Ok();
        }

        [HttpPost("register")] // "api/users/register"
        public ActionResult<User> Register(CreateUserDto createUserDto)
        {
            if (_userService.EmailRegistered(createUserDto.Email)) return Conflict();
            User user = FromCreateDto(createUserDto);
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

        private int getCurrentUserId()
        {
            string idAsString = (from c in HttpContext.User.Claims
                                 where c.Type == "id"
                                 select c.Value).FirstOrDefault();
            if(idAsString == null)
            {
                //Throw EXCEPTION
            }
            return idAsString != null ? int.Parse(idAsString) : 0;
        }
    }
}