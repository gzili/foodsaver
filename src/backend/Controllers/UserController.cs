using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using backend.DTO.Address;
using backend.DTO.Offers;
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
            if (!_userService.IsValidRegister(createUserDto))
                return Conflict("User with the same email already exists");
            
            var user = _userService.FromCreateDto(createUserDto);
            _userService.Save(user);
            
            return Ok(ToDto(user));
        }
        
        [HttpPost("login")] // "api/user/login"
        public ActionResult<User> Login(LoginUserDto loginUserDto)
        {
            var user = _userService.CheckLogin(loginUserDto);
            if (user == null) return Unauthorized();
            
            var claims = new List<Claim> { new("id", user.Id.ToString()) };
            HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme, "id", "")));
            
            return Ok(ToDto(user));
        }

        [Authorize]
        [HttpPost("logout")] // "api/user/logout"
        public IActionResult LogOut()
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
            return Ok(ToDto(user));
        }
        
        [Authorize]
        [HttpGet("offers")] // "api/user/offers"
        public IEnumerable<OfferDto> FindByUser()
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);
            return _userService.GetOffersByUserId(userId).Select(_offersService.ToDto).ToList();
        }

        private UserDto ToDto(User user) => new()
        {
            Id = user.Id,
            UserType = user.UserType,
            Name = user.Username,
            Email = user.Email,
            Address = new AddressDto
            {
                StreetAddress = user.Address.Street,
                City = user.Address.City
            }
        };
    }
}