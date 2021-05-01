using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using AutoMapper;
using Microsoft.AspNetCore.Mvc.Versioning;
using Discount.Grpc.Protos;
using Basket.Api.GrpcServices;
using MassTransit;

namespace Basket.Api
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IBasketRepository, BasketRepository>();
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);


            services.AddApiVersioning(options =>
          {
              options.AssumeDefaultVersionWhenUnspecified = true;
              options.DefaultApiVersion = new ApiVersion(1, 0);
              options.ReportApiVersions = true;
              options.ApiVersionReader = new HeaderApiVersionReader("api-version");
          });

            services.AddMyGrpcClient(Configuration);
            services.AddMyMassTransit(Configuration);

            services.AddMyRedisCache(Configuration);
            //once u attach a shell run redis-cli
            //then keys *

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

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

    public static class ServiceExtensions
    {
        public static IServiceCollection AddMyRedisCache(this IServiceCollection services, IConfiguration config)
        {
            //docker run -d -p 6379:6379 --name redis_basket redis
            var redisServer = config["REDIS_SERVER"] ?? "localhost";
            var redisConnection = $"{redisServer}:6379";
            Console.WriteLine($"CONNECTION STRING Basket redis: {redisConnection}");
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnection;
            });
            return services;
        }

        public static IServiceCollection AddMyMassTransit(this IServiceCollection services, IConfiguration config)
        {
            services.AddMassTransit(configuration =>
            {
                configuration.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(config["EventBusAddress"]);
                });
            });
            services.AddMassTransitHostedService();
            return services;
        }

        public static IServiceCollection AddMyGrpcClient(this IServiceCollection services, IConfiguration config)
        {
            Console.WriteLine($"Grpc url: {config["grpcServiceUrl"]}");
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt =>
            {
                opt.Address = new Uri(config["grpcServiceUrl"]);

            });
            services.AddScoped<DiscountGrpcService>();
            services.AddControllers();
            return services;
        }

    }
}
