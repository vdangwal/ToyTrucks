using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using StackExchange.Redis;

using AutoMapper;
using Microsoft.AspNetCore.Mvc.Versioning;
using Discount.Grpc.Protos;
using Basket.Api.GrpcServices;
using Basket.Api.Events;
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
            services.AddHttpContextAccessor();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IBasketRepository, RedisBasketRepository>();
            services.AddTransient<IIdentityService, IdentityService>();
            // services.AddScoped<IBasketLinesService, BasketLinesService>();
            // // services.AddScoped<OLDIBasketContext, OLDBasketContext>();
            // services.AddScoped<ITruckService, TruckService>();

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);


            services.AddApiVersioning(options =>
          {
              options.AssumeDefaultVersionWhenUnspecified = true;
              options.DefaultApiVersion = new ApiVersion(1, 0);
              options.ReportApiVersions = true;
              options.ApiVersionReader = new HeaderApiVersionReader("api-version");
          });

            var requireAuthenticatedUserPolicy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();

            services.AddMyGrpcClient(Configuration);
            //  services.AddMyRedisCache(Configuration);
            services.AddMyMassTransit(Configuration);

            //  services.AddHttpClient<ITruckCatalogApiService, TruckCatalogApiService>(c =>
            //c.BaseAddress = new Uri(Configuration["TruckCatalogUri"]));

            services.AddSingleton<ConnectionMultiplexer>(sp =>
                       {
                           //    /var settings = sp.GetRequiredService<IOptions<BasketSettings>>().Value;
                           var configuration = ConfigurationOptions.Parse(Configuration["RedisServerUrl"], true);

                           configuration.ResolveDns = true;

                           return ConnectionMultiplexer.Connect(configuration);
                       });

            services.AddControllers(config =>
            {
                config.Filters.Add(new AuthorizeFilter(requireAuthenticatedUserPolicy));
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:3520";
                    options.Audience = "hesstoytrucks";
                });
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class ServiceExtensions
    {
        public static IServiceCollection AddMyGrpcClient(this IServiceCollection services, IConfiguration config)
        {
            Console.WriteLine($"Grpc url: {config["grpcServiceUrl"]}");
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt =>
            {
                opt.Address = new Uri(config["grpcServiceUrl"]);

            });
            services.AddScoped<DiscountGrpcService>();

            return services;
        }

        public static IServiceCollection AddMyMassTransit(this IServiceCollection services, IConfiguration config)
        {
            Console.WriteLine($"EventBusAddress: {config["EventBusAddress"]}");

            services.AddMassTransit(configuration =>
            {
                configuration.AddConsumer<UpdatedInventoryConsumer>();
                //create new service bus
                configuration.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(config["EventBusAddress"]);
                    cfg.ReceiveEndpoint(config["BasketUpdatedQueue"], c =>
                   {
                       c.ConfigureConsumer<UpdatedInventoryConsumer>(ctx);
                   });
                });
            });
            services.AddMassTransitHostedService();

            return services;

        }

        // public static IServiceCollection AddMyRedisCache(this IServiceCollection services, IConfiguration config)
        // {
        //     //docker run -d -p 6379:6379 --name redis_basket redis
        //     var redisServer = config["REDIS_SERVER"] ?? "localhost";
        //     var redisConnection = $"{redisServer}:6379";
        //     Console.WriteLine($"CONNECTION STRING Basket redis: {redisConnection}");
        //     services.AddStackExchangeRedisCache(options =>
        //     {
        //         options.Configuration = redisConnection;
        //     });
        //     return services;

        //     //once u attach a shell run redis-cli
        // 127.0.0.1:6379> keys *
        // 1) "a3597eca-d535-40e9-bc8f-08d9542dcc7d"
        // 127.0.0.1:6379> get a3597eca-d535-40e9-bc8f-08d9542dcc7d
        // ...
        // 127.0.0.1:6379> del a3597eca-d535-40e9-bc8f-08d9542dcc7d

    }
}
