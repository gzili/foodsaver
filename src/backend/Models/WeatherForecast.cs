using System;

namespace backend.Models
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 31 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
