//
//            God Bless         No Bugs
//
//
//
//                      _oo0oo_
//                     o8888888o
//                     88" . "88
//                     (| -_- |)
//                     0\  =  /0
//                   ___/`---'\___
//                 .' \\|     |// '.
//                / \\|||  :  |||// \
//               / _||||| -:- |||||- \
//              |   | \\\  -  /// |   |
//              | \_|  ''\---/''  |_/ |
//              \  .-\__  '-'  ___/-. /
//            ___'. .'  /--.--\  `. .'___
//         ."" '<  `.___\_<|>_/___.' >' "".
//        | | :  `- \`.;`\ _ /`;.`/ - ` : | |
//        \  \ `_.   \_ __\ /__ _/   .-` /  /
//    =====`-.____`.___ \_____/___.-`___.-'=====
//                      `=---='
//
//  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//

using AspNetCore.Caching.Demo.Config;
using AspNetCore.Caching.Demo.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AspNetCore.Caching.Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AspNetCore.Caching.Demo", Version = "v1"
                });
            });

            DatabaseConfig.Setup(services, Configuration);
            services.AddMemoryCache();

            services.AddDistributedMemoryCache();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
            });

            services
                .AddHealthChecks()
                .AddRedis(Configuration.GetConnectionString("Redis"));

            services.AddHostedService<AppInitializeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = string.Empty;
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AspNetCore.Caching.Demo v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            HealthCheckCustomResponse.Setup(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
