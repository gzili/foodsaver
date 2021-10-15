using System.Collections.Generic;
using System.Security.Claims;
using backend.DTO.Users;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BC = BCrypt.Net.BCrypt;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<User> Get() // "api/users"
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);
            var user = _userService.GetById(userId);
            return Ok(user);
        }

        [HttpPost("login")] // "api/users/login"
        public ActionResult<User> Login(LoginUserDto loginUserDto)
        {
            var user = _userService.CheckLogin(loginUserDto);
            if (user == null) return Unauthorized();
            var claims = new List<Claim> {new("id", user.Id.ToString())};
            HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme, "id", "")));
            return NoContent();
        }

        [Authorize]
        [HttpPost("logout")] // "api/users/logout"
        public ActionResult LogOut()
        {
            HttpContext.SignOutAsync();
            return Ok();
        }

        [HttpPost("register")] // "api/users/register"
        public ActionResult<User> Register(CreateUserDto createUserDto)
        {
            if (_userService.EmailRegistered(createUserDto.Email)) return Conflict();
            var user = FromCreateDto(createUserDto);
            _userService.Save(user);
            return Ok(user);
        }

        public User FromCreateDto(CreateUserDto createUserDto)
        {
            return new User(
                _userService.GetAll().Count + 1,
                createUserDto.Email,
                createUserDto.Name,
                BC.HashPassword(createUserDto.Password),
                createUserDto.Address,
                createUserDto.UserType
            );
        }
    }
}