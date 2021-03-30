using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore.Caching.Demo.Models
{
    public class WeatherForecast : BaseModel
    {
        public DateTime Date { get; protected set; }

        public int TemperatureC { get; protected set; }

        [NotMapped]
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [StringLength(100)]
        public string Summary { get; protected set; }

        public WeatherForecast(DateTime date, int temperatureC, string summary)
        {
            Date = date;
            TemperatureC = temperatureC;
            Summary = summary;
        }

        protected WeatherForecast()
        {
        }
    }
}
