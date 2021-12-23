using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToyTrucks.Basket.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Versioning;
using ToyTrucks.Basket.Api.Events;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.IdentityModel.Tokens.Jwt;
using ToyTrucks.Basket.Api.Helpers;
using RabbitMQ.Client;
using EventBus.Messages.Common;
namespace ToyTrucks.Basket.Api
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
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddHttpContextAccessor();
            services.AddAccessTokenManagement();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IBasketRepository, RedisBasketRepository>();
            services.AddScoped<TokenExchangeService>();
            services.AddTransient<IIdentityService, IdentityService>();

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            services.AddApiVersioning(options =>
          {
              options.AssumeDefaultVersionWhenUnspecified = true;
              options.DefaultApiVersion = new ApiVersion(1, 0);
              options.ReportApiVersions = true;
              options.ApiVersionReader = new HeaderApiVersionReader("api-version");
          });

            services.AddMyMassTransit(Configuration);


            services.AddSingleton<ConnectionMultiplexer>(sp =>
            {
                //    /var settings = sp.GetRequiredService<IOptions<BasketSettings>>().Value;
                var configuration = ConfigurationOptions.Parse(Configuration["RedisServerUrl"], true);

                configuration.ResolveDns = true;

                return ConnectionMultiplexer.Connect(configuration);
            });
            System.Console.WriteLine($"BAsket Service useAuth = {Configuration["UseOAuth"]}");
            if (Configuration["UseOAuth"] == "true")
            {
                var requireAuthenticatedUserPolicy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();

                var builder = services.AddControllers(options =>
                {
                    options.Filters.Add(new AuthorizeFilter(requireAuthenticatedUserPolicy));
                });

                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Audience = "basket";
                        options.Authority = Configuration["IdentityServerUrl"];
                    });
            }
            else
            {
                services.AddControllers();
            }
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
            if (Configuration["UseOAuth"] == "true")
            {
                app.UseAuthentication();
            }
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class ServiceExtensions
    {
        public static IServiceCollection AddMyMassTransit(this IServiceCollection services, IConfiguration config)
        {
            var queueSettingsSection = new QueueSettings();
            config.Bind("RabbitMQ:QueueSettings", queueSettingsSection);

            services.AddMassTransit(configuration =>
            {
                configuration.AddConsumer<UpdatedInventoryConsumer>();
                //create new service bus
                configuration.UsingRabbitMq((ctx, cfg) =>
                {
                    //  var rabbitUri = $"amqp://{queueSettingsSection["UserName"]}:{queueSettingsSection["Password"]}@{queueSettingsSection["HostName"]}:{queueSettingsSection["Port"]}{queueSettingsSection["VirtualHost"]}";
                    //cfg.Host(rabbitUri);//config["EventBusAddress"]);
                    cfg.Host(queueSettingsSection.HostName, queueSettingsSection.VirtualHost,
                 //cfg.Host("localhost", queueSettingsSection.VirtualHost,
                 cg =>
                 {
                     cg.Username(queueSettingsSection.UserName);
                     cg.Password(queueSettingsSection.Password);
                 });
                    // cfg.ExchangeType = ExchangeType.Direct;
                    cfg.ReceiveEndpoint(config["BasketUpdatedQueue"], c =>
                   {
                       c.ConfigureConsumer<UpdatedInventoryConsumer>(ctx);
                   });
                });
            });
            services.AddMassTransitHostedService();

            return services;
        }
    }
}
