using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly UsersService _usersService;
        private readonly FileUploadService _fileUploadService;

        public UserController(IMapper mapper, UsersService usersService, FileUploadService fileUploadService)
        {
            _mapper = mapper;
            _usersService = usersService;
            _fileUploadService = fileUploadService;
        }

        [HttpPost("register")] // "api/user/register"
        public async Task<ActionResult<User>> RegisterAsync([FromForm] CreateUserDto createUserDto)
        {
            if (_usersService.GetByEmail(createUserDto.Email) != null)
                return Conflict("User with the same email already exists");

            var avatarPath = await _fileUploadService.UploadFormFileAsync(createUserDto.Avatar, "images");

            if (avatarPath == null)
                return BadRequest("Invalid image file");

            var user = _mapper.Map<User>(createUserDto);
            user.AvatarPath = avatarPath;
            _usersService.Create(user);
            
            return Ok(_mapper.Map<UserDto>(user));
        }
        
        [HttpPost("login")] // "api/user/login"
        public ActionResult<User> Login(LoginUserDto loginUserDto)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return Conflict("User is already authenticated");
            
            var user = _usersService.IsValidLogin(loginUserDto.Email, loginUserDto.Password);
            if (user == null) return Unauthorized();
            
            var claims = new List<Claim> { new("id", user.Id.ToString()) };
            HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme, "id", "")));
            
            return Ok(_mapper.Map<UserDto>(user));
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
            var user = (User) HttpContext.Items["user"];
            return Ok(_mapper.Map<UserDto>(user));
        }
        
        [Authorize]
        [HttpGet("offers")] // "api/user/offers"
        public IEnumerable<OfferDto> FindByUser()
        {
            var user = (User) HttpContext.Items["user"];
            return _usersService.GetOffersByUserId(user.Id).Select(_mapper.Map<OfferDto>).ToList();
        }
    }
}