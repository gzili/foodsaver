using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<WeatherForecast> WeatherForecastSet { get; set; }
    }
}