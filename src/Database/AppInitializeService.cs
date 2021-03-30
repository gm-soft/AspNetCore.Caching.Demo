using System;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Caching.Demo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Caching.Demo.Database
{
    // https://github.com/maximgorbatyuk/MG.Utils/blob/main/src/MG.Utils.AspNetCore/HostedServices/AppInitializeServiceBase.cs
    public class AppInitializeService : IHostedService
    {
        protected ILogger Logger { get; }

        private readonly IServiceProvider _serviceProvider;

        public AppInitializeService(ILogger<AppInitializeService> logger, IServiceProvider serviceProvider)
        {
            Logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            await context.Database.MigrateAsync(cancellationToken);

            if (!await context.WeatherForecasts.AnyAsync(cancellationToken))
            {
                await context.AddRangeAsync(new WeatherForecastRandomCollection(1, 20), cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}