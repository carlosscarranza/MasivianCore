using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Application.Core.Implementation;
using Application.Core.Implementation.Validations;
using FluentValidation.AspNetCore;
using Infrastructure.Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Converters;
using Utilities.Core.Implementation.Database;
using Utilities.Core.Implementation.Middleware;

namespace DistributedServices.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION");

            services.AddDbContext<CoreContext>(
                options => options.UseSqlServer(connectionString ?? throw new InvalidOperationException()),
                ServiceLifetime.Scoped);

            services.UseRepository(typeof(CoreContext));

            services.UseApplication();

            services.AddControllers()
                .AddNewtonsoftJson(options => {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .AddFluentValidation(fv => fv
                    .RegisterValidatorsFromAssemblyContaining<CreateBetValidation>()
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
