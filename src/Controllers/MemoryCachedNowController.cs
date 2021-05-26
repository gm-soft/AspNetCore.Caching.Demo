using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Caching.Demo.Controllers
{
    [ApiController]
    [Route("date/now/memory-cached")]
    public class MemoryCachedNowController : ControllerBase
    {
        private readonly ILogger<MemoryCachedNowController> _logger;
        private readonly IMemoryCache _memoryCache;

        public MemoryCachedNowController(ILogger<MemoryCachedNowController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            const string nowKey = "MemoryCachedNowController_Index";
            return _memoryCache.GetOrCreate<IActionResult>(nowKey, entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(3);
                return new DateTimeActionResult(_logger, DateTimeOffset.Now);
            });
        }
    }
}