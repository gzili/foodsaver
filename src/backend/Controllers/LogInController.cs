using System;
using System.Threading;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogInController : Controller
    {
        private readonly LogInService _logInService;

        public LogInController()
        {
            _logInService = new LogInService();
        }
        
        [HttpGet]
        public User GetLogin()
        {
            return _logInService.Get();
        }
        
        [HttpPost]
        public void PostLogin(User user)
        {
            _logInService.Save(user);
            Console.WriteLine("------");
            foreach (User user1 in _logInService.GetAll())
            {
                Console.WriteLine(user1.ToString());
            }
        }
    }
}