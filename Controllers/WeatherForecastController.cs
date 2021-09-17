using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using FoodSaver.Model;
using FoodSaver.Services;

namespace FoodSaver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly WeatherForeCastService _weatherForeCastService;
        
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _weatherForeCastService = new WeatherForeCastService();
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _weatherForeCastService.GetWeatherForecasts();
        }
    }
}
