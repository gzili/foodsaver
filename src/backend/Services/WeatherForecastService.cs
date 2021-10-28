using System.Collections.Generic;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class WeatherForecastService : IService<WeatherForecast>
    {
        private readonly WeatherForecastRepository _weatherForecastRepository;

        public WeatherForecastService(WeatherForecastRepository weatherForecastRepository)
        {
            _weatherForecastRepository = weatherForecastRepository;
        }

        public WeatherForecast GetById(int id)
        {
            return _weatherForecastRepository.GetById(id);
        }

        public List<WeatherForecast> GetAll()
        {
            return _weatherForecastRepository.GetAll();
        }

        public void Save(WeatherForecast t)
        {
            _weatherForecastRepository.Save(t);
        }
    }
}