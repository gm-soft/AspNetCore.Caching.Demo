using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Caching.Demo.Controllers
{
    [ApiController]
    [Route("date")]
    public class DateTimeController : ControllerBase
    {
        private readonly ILogger<DateTimeController> _logger;
        private readonly IMemoryCache _memoryCache;

        public DateTimeController(ILogger<DateTimeController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        [HttpGet("now")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Now() => NowInternal();

        [HttpGet("now/memory-cached")]
        public IActionResult MemoryCachedNow()
        {
            const string nowKey = "DateTimeController_MemoryCachedNow";
            return _memoryCache.GetOrCreate<IActionResult>(nowKey, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(10);
                return NowInternal();
            });

        }

        // https://metanit.com/sharp/aspnet5/14.2.php
        // https://stackoverflow.com/a/52399029
        [HttpGet("now/response-cached")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 10)]
        public IActionResult ResponseCached() => NowInternal();

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