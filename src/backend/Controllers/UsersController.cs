using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

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
        
        /*
        [HttpGet("{id:int}")] // "api/users/<number>"
        public User Get(int id)
        {
            return _userService.GetById(id);
        }*/

        [HttpPost("login")] // "api/users/login"
        public ActionResult<User> Login(User user)
        {
            if (_userService.CheckLogin(user))
            {
                return Ok(user);
            }
            return NotFound();
        }

        [HttpPost("register")] // "api/users/register"
        public ActionResult Register(User user)
        {
            if (!_userService.CheckRegister(user)) return Conflict();
            _userService.Save(user);
            return Ok();
        }
    }
}