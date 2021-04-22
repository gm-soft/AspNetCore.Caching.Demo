using System;
using AspNetCore.Caching.Demo.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Caching.Demo.Controllers
{
    public class DateTimeActionResult : OkObjectResult
    {
        public DateTimeActionResult(ILogger logger, DateTimeOffset date)
            : base(new DateTimeDto(date))
        {
            logger.LogInformation("Current date is requested");
        }
    }
}