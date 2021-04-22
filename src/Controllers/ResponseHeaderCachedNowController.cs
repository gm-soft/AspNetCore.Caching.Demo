using System;
using System.Text.Json;
using System.Threading.Tasks;
using AspNetCore.Caching.Demo.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Caching.Demo.Controllers
{
    [ApiController]
    [Route("date")]
    public class ResponseHeaderCachedNowController : ControllerBase
    {
        private readonly ILogger<ResponseHeaderCachedNowController> _logger;

        public ResponseHeaderCachedNowController(ILogger<ResponseHeaderCachedNowController> logger)
        {
            _logger = logger;
        }

        // https://metanit.com/sharp/aspnet5/14.2.php
        // https://stackoverflow.com/a/52399029
        [HttpGet("now/response-cached")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 10)]
        public IActionResult ResponseCached()
        {
            return new DateTimeActionResult(_logger, DateTimeOffset.Now);
        }

        [HttpGet("now/response-no-cached")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ResponseNoCached()
        {
            return new DateTimeActionResult(_logger, DateTimeOffset.Now);
        }

        [HttpGet("now/response-vary-cached")]
        [ResponseCache(Location = ResponseCacheLocation.Any, VaryByHeader = "User-Agent", Duration = 300)]
        public IActionResult ResponseCachedUserAgent()
        {
            return new DateTimeActionResult(_logger, DateTimeOffset.Now);
        }
    }
}