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
    public class UserController : Controller
    {
        private readonly OffersService _offersService;
        private readonly UserService _userService;

        public UserController(OffersService offersService, UserService userService)
        {
            _offersService = offersService;
            _userService = userService;
        }

        [HttpPost("register")] // "api/user/register"
        public ActionResult<User> Register(CreateUserDto createUserDto)
        {
            if (!_userService.Validate(createUserDto)) return Conflict();
            if (!_userService.IsValidRegister(createUserDto)) return Conflict();
            var user = _userService.FromCreateDto(createUserDto);
            _userService.Save(user);
            return Ok(user);
        }
        
        [HttpPost("login")] // "api/user/login"
        public ActionResult<User> Login(LoginUserDto loginUserDto)
        {
            var user = _userService.CheckLogin(loginUserDto);
            if (user == null) return Unauthorized();
            var claims = new List<Claim> { new("id", user.Id.ToString()) };
            HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme, "id", "")));
            return Ok(user);
        }

        [Authorize]
        [HttpPost("logout")] // "api/user/logout"
        public ActionResult LogOut()
        {
            HttpContext.SignOutAsync();
            return Ok();
        }
        
        [Authorize]
        [HttpGet]
        public ActionResult<User> Get() // "api/user"
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);
            var user = _userService.GetById(userId);
            return Ok(user);
        }
        
        [Authorize]
        [HttpGet("offers")] // "api/user/offers"
        public IEnumerable<Offer> FindByUser()
        {
            return _offersService.GetByUserId(int.Parse(HttpContext.User.Identity.Name));
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