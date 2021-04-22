using System;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Caching.Demo.Models;
using MG.Utils.AspNetCore.HostedServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Caching.Demo.Database
{
    public class AppInitializeService : AppInitializeServiceBase
    {
        public AppInitializeService(ILogger<AppInitializeService> logger, IServiceProvider serviceProvider)
            : base(logger, serviceProvider)
        {
        }

        protected override async Task InitAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            await context.Database.MigrateAsync(cancellationToken);

            if (!await context.WeatherForecasts.AnyAsync(cancellationToken))
            {
                await context.AddRangeAsync(new WeatherForecastRandomCollection(1, 20), cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}