using System.Collections.Generic;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherForecastService _wfService;
        
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherForecastService wfService)
        {
            _logger = logger;
            _wfService = wfService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> GetAll()
        {
            return _wfService.GetAll();
        }
        
        [HttpGet("{id:int}")]
        public WeatherForecast Get(int id)
        {
            return _wfService.GetById(id);
        }
        
        [HttpPost("add")]
        public ActionResult<WeatherForecast> Get(WeatherForecast weatherForecast)
        {
            _wfService.Save(weatherForecast);
            return Ok(weatherForecast);
        }
    }
}