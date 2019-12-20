using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharesAPI.DatabaseAccess;
using SharesAPI.Options;

namespace SharesAPI
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
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ExpressDBConnection"))); //If there is already an instance of AppDbContext in pool, use this instance, instead of creating a new one.
            //Create options to use SqlServer
            //Get Connection String set in appsettings.json

            //Dependency Injection of the SqlRepo implementation for SqlStockRepository
            //Want SqlStockRepository to be available through the entire scope of a given HTTP request
            //When a new request is made, we want a new instance of SqlStockRepostory completed
            services.AddScoped<IStockRepository, SqlStockRepositroy>();

            //Differences between AddTransient, AddScoped and AddSingleton at:
            //https://www.youtube.com/watch?v=v6Nr7Zman_Y&list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU&index=44

            //https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.0&tabs=visual-studio
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Stock API", Version = "V1" });
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            //Requirese Swashbuckle.AspNetCore nuget package For netcore 3 requires 5.0.0-rc4
            app.UseSwagger(option =>
            {
                option.RouteTemplate = swaggerOptions.JsonRoute;

            });

            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
            });
        }
    }
}
