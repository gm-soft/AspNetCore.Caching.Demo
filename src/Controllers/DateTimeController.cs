using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Caching.Demo.Controllers
{
    [ApiController]
    [Route("date")]
    public class DateTimeController : ControllerBase
    {
        private const int TenSeconds = 10;
        private static readonly TimeSpan _tenSeconds = TimeSpan.FromSeconds(TenSeconds);

        private readonly ILogger<DateTimeController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;

        public DateTimeController(ILogger<DateTimeController> logger, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
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
                entry.SlidingExpiration = _tenSeconds;
                return NowInternal();
            });

        }

        [HttpGet("now/redis-cached")]
        public async Task<IActionResult> RedisCachedNowAsync()
        {
            const string nowKey = "DateTimeController_MemoryCachedNow";

            string serialized = await _distributedCache.GetStringAsync(nowKey);
            if (serialized != null)
            {
                _logger.LogInformation("From redis");
                return Ok(JsonSerializer.Deserialize<NowResponse>(serialized));
            }

            _logger.LogInformation("Current date is requested");
            var response = new NowResponse(DateTimeOffset.Now);

            await _distributedCache.SetStringAsync(
                key: nowKey,
                value: JsonSerializer.Serialize(response),
                options: new DistributedCacheEntryOptions
                {
                    SlidingExpiration = _tenSeconds
                });

            return Ok(response);

        }

        // https://metanit.com/sharp/aspnet5/14.2.php
        // https://stackoverflow.com/a/52399029
        [HttpGet("now/response-cached")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = TenSeconds)]
        public IActionResult ResponseCached() => NowInternal();

        private IActionResult NowInternal()
        {
            _logger.LogInformation("Current date is requested");
            var now = DateTimeOffset.Now;
            return Ok(new NowResponse(DateTimeOffset.Now));
        }

        public class NowResponse
        {
            public DateTime Now { get; init; }

            public int TimeZone { get; init; }

            public long Ms { get; init; }

            public NowResponse(DateTimeOffset time)
            {
                Now = time.DateTime;
                TimeZone = time.Offset.Hours;
                Ms = time.Millisecond;
            }

            public NowResponse()
            {
            }
        }
    }
}