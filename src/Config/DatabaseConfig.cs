using System;
using AspNetCore.Caching.Demo.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Caching.Demo.Config
{
    public static class DatabaseConfig
    {
        public static void Setup(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext<DatabaseContext>((DbContextOptionsBuilder optionsBuilder) =>
                        optionsBuilder
                            .UseSqlite(configuration.GetConnectionString("Db")));
        }
    }
}