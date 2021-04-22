using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Caching.Demo.Controllers
{
    [ApiController]
    [Route("date/now")]
    public class NowController : ControllerBase
    {
        private readonly ILogger<NowController> _logger;

        public NowController(ILogger<NowController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index()
        {
            return new DateTimeActionResult(_logger, DateTimeOffset.Now);
        }
    }
}