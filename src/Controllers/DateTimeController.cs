using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Caching.Demo.Controllers
{
    [ApiController]
    [Route("date")]
    public class DateTimeController : ControllerBase
    {
        private readonly ILogger<DateTimeController> _logger;

        public DateTimeController(ILogger<DateTimeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("now")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Now() => NowInternal();

        // https://metanit.com/sharp/aspnet5/14.2.php
        // https://stackoverflow.com/a/52399029
        [HttpGet("now/response-cached")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 10)]
        public IActionResult NowCached() => NowInternal();

        private IActionResult NowInternal()
        {
            _logger.LogInformation("Current date is requested");
            var now = DateTimeOffset.Now;
            return Ok(new
            {
                Now = now.DateTime,
                TimeZone = now.Offset.Hours,
                Ms = now.Millisecond
            });
        }
    }
}