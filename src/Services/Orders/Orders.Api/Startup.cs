using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Orders.Api.DBContexts;
using Orders.Api.Services;
using MassTransit;
using EventBus.Messages.Common;
using Orders.Api.Entities;

namespace Orders.Api
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders", Version = "v1" });
            });

            services.AddPostgresDbContext(Configuration);
            services.AddScoped<IOrdersRepository, OrdersRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<BasketCheckoutConsumer>();

            services.AddClientMassTransit(Configuration);

            services.AddApiVersioning(options =>
          {
              options.AssumeDefaultVersionWhenUnspecified = true;
              options.DefaultApiVersion = new ApiVersion(1, 0);
              options.ReportApiVersions = true;
              options.ApiVersionReader = new HeaderApiVersionReader("api-version");
          });

            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders v1"));
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class ServiceCollectionExtensionMethods
    {
        public static IServiceCollection AddPostgresDbContext(this IServiceCollection services, IConfiguration config)
        {

            //Create  postgres container
            //docker run -e POSTGRES_DB=orderdb -e POSTGRES_USER=marcus -e POSTGRES_PASSWORD=password -p 5432:5430 --name postgres_order -d postgres
            //run bash to access postgres container
            //docker exec -it 480c32e2bb53 "bash" //where 480c3.. is the container id

            //psql -h localhost -p 5432 -U marcus -d orderdb
            var server = config["POSTGRES_SERVER"];// ?? "localhost";

            var port = config["POSTGRES_PORT"];// ?? "5432";
            var database = config["POSTGRES_DB"] ?? "orderdb";


            var user = config["POSTGRES_USER"];// ?? "marcus";
            var password = config["POSTGRES_PASSWORD"];// ?? "password";

            var connectionString = $"Host={server}; Port={port}; Database={database}; Username={user}; Password={password};";
            Console.WriteLine($"CONNECTION STRING Order: {connectionString}");

            services.AddDbContext<OrderDbContext>(options =>
                options.UseNpgsql(connectionString)
                       .UseSnakeCaseNamingConvention()
                    );
            return services;
        }

        public static IServiceCollection AddClientMassTransit(this IServiceCollection services, IConfiguration config)
        {
            Console.WriteLine($"EventBusAddress: {config["EventBusAddress"]}");

            services.AddMassTransit(configuration =>
            {

                configuration.AddConsumer<BasketCheckoutConsumer>();
                configuration.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(config["EventBusAddress"]);
                    cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
                    {
                        c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
                    });
                });
            });
            services.AddMassTransitHostedService();

            return services;

        }

    }
}
