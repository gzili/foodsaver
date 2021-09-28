using System;
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
        [HttpGet]
        public User GetLogin()
        {
            return _userService.get();
        }*/
        
        [HttpPost("login")]
        public void PostLogin(User user)
        {
            _userService.Save(user);
            Console.WriteLine("------");
            foreach (User user1 in _userService.GetAll())
            {
                Console.WriteLine(user1.ToString());
            }
        }
        
        /*
        [HttpGet]
        public User GetRegister()
        {
            return _userService.ge();
        } */
        
        [HttpPost("register")]
        public void PostRegister(User user)
        {
            Console.WriteLine(user.ToString());
            _userService.Save(user);
        }
    }
}