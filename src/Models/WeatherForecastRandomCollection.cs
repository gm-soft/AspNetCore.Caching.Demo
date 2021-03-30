using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCore.Caching.Demo.Models
{
    public class WeatherForecastRandomCollection : IReadOnlyCollection<WeatherForecast>
    {
        private static readonly string[] _summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IReadOnlyCollection<WeatherForecast> _collection;

        public WeatherForecastRandomCollection(int @from, int to)
        {
            var random = new Random();

            _collection = Enumerable.Range(@from, to).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = random.Next(-20, 55),
                Summary = _summaries[random.Next(_summaries.Length)]
            }).ToArray();
        }

        public IEnumerator<WeatherForecast> GetEnumerator() => _collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _collection.Count;
    }
}