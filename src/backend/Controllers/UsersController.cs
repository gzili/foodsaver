using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using backend.DTO.User;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BC = BCrypt.Net.BCrypt;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // /api/users
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IFileService _fileService;

        public UsersController(
            IUsersService usersService,
            IMapper mapper,
            IConfiguration config,
            IFileService fileService)
        {
            _usersService = usersService;
            _mapper = mapper;
            _config = config;
            _fileService = fileService;
        }

        [HttpPost("register")] // POST /api/users/register
        public async Task<ActionResult<UserDto>> RegisterAsync([FromForm] CreateUserDto createUserDto)
        {
            if (_usersService.GetByEmail(createUserDto.Email) != null)
                return Conflict("User with the same email already exists");

            var avatarPath = await _fileService.UploadFormFileAsync(createUserDto.Avatar, _config["UploadedFilesPath"]);

            var user = _mapper.Map<User>(createUserDto);
            user.AvatarPath = avatarPath;
            _usersService.Create(user);
            
            return _mapper.Map<UserDto>(user);
        }
        
        [HttpPost("login")] // POST /api/users/login
        public ActionResult<UserDto> Login(LoginUserDto loginUserDto)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return Conflict("User is already authenticated");
            
            var user = _usersService.IsValidLogin(loginUserDto.Email, loginUserDto.Password);
            if (user == null) return Unauthorized();
            
            var claims = new List<Claim> { new(ClaimTypes.Name, user.Id.ToString())};
            HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme)));
            
            return _mapper.Map<UserDto>(user);
        }

        [Authorize]
        [HttpPost("logout")] // POST /api/users/logout
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync();
            
            return Ok();
        }

        [HttpGet("{id:int}")]
        public PlaceDto Get(int id)
        {
            return _usersService.GetById(id);
        }
    }
}