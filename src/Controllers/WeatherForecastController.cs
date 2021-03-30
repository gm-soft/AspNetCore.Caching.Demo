using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCore.Caching.Demo.Database;
using AspNetCore.Caching.Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Caching.Demo.Controllers
{
    [ApiController]
    [Route("/weather-forecasts")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly DatabaseContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("")]
        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            return await _context.WeatherForecasts
                .AsNoTracking()
                .ToArrayAsync();
        }

        [HttpGet("memory-cache")]
        public async Task<IEnumerable<WeatherForecast>> FromMemoryCacheAsync()
        {
            return await _context.WeatherForecasts
                .AsNoTracking()
                .ToArrayAsync();
        }
    }
}
