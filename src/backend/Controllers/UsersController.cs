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
        private readonly FileUploadService _fileUploadService;
        private readonly IMapper _mapper;
        private readonly UsersService _usersService;

        private readonly string _uploadPath;

        public UsersController(FileUploadService fileUploadService, IConfiguration config, IMapper mapper, UsersService usersService)
        {
            _fileUploadService = fileUploadService;
            _mapper = mapper;
            _usersService = usersService;
            
            _uploadPath = config["UploadedFilesPath"];
        }

        [HttpPost("register")] // POST /api/users/register
        public async Task<ActionResult<UserDto>> RegisterAsync([FromForm] CreateUserDto createUserDto)
        {
            if (_usersService.GetByEmail(createUserDto.Email) != null)
                return Conflict("User with the same email already exists");

            var avatarPath = await _fileUploadService.UploadFormFileAsync(createUserDto.Avatar, _uploadPath);

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
    }
}