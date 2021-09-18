using System;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        private readonly RegisterService _registerService;
        
        public RegisterController()
        {
            _registerService = new RegisterService();
        }

        [HttpGet]
        public User GetRegister()
        {
            return _registerService.GetUser();
        } 
        
        [HttpPost]
        public void PostRegister(User user)
        {
            Console.WriteLine(user.ToString());
            _registerService.Save(user);
        }
    }
}