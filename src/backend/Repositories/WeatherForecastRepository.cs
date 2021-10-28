using System.Collections.Generic;
using System.Linq;
using backend.Data;
using backend.Models;

namespace backend.Repositories
{
    public class WeatherForecastRepository
    {
        private readonly AppDbContext _appDbContext;

        public WeatherForecastRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Save(WeatherForecast t)
        {
            _appDbContext.WeatherForecastSet.Add(t);
            _appDbContext.SaveChanges();
        }

        public WeatherForecast GetById(int id)
        {
            return _appDbContext.WeatherForecastSet.FirstOrDefault(x => x.Id == id);
        }

        public List<WeatherForecast> GetAll()
        {
            return new List<WeatherForecast>(_appDbContext.WeatherForecastSet);
        }
    }
}