using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("weather_forecast")]
    public class WeatherForecast
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string Summary { get; set; }
    }
}