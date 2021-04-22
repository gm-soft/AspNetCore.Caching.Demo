using System;
using System.Text.Json;
using System.Threading.Tasks;
using AspNetCore.Caching.Demo.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Caching.Demo.Controllers
{
    [ApiController]
    [Route("date/now/redis-cached")]
    public class RedisCachedNowController : ControllerBase
    {
        private readonly ILogger<RedisCachedNowController> _logger;
        private readonly IDistributedCache _distributedCache;

        public RedisCachedNowController(ILogger<RedisCachedNowController> logger, IDistributedCache distributedCache)
        {
            _logger = logger;
            _distributedCache = distributedCache;
        }

        [HttpGet("")]
        public async Task<IActionResult> IndexAsync()
        {
            const string nowKey = "RedisCachedNowController_IndexAsync";

            string serialized = await _distributedCache.GetStringAsync(nowKey);
            if (serialized != null)
            {
                _logger.LogInformation("From redis");
                return Ok(JsonSerializer.Deserialize<DateTimeDto>(serialized));
            }

            _logger.LogInformation("Current date is requested");
            var response = DateTimeDto.Now();

            await _distributedCache.SetStringAsync(
                key: nowKey,
                value: JsonSerializer.Serialize(response),
                options: new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(3)
                });

            return Ok(response);
        }
    }
}