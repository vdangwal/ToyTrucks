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
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Orders.Api.Entities;
using MongoDB;
using Orders.Api.Helpers;

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
            var requireAuthenticatedUserPolicy = new AuthorizationPolicyBuilder()
                                   .RequireAuthenticatedUser()
                                   .Build();
            services.AddControllers(config =>
            {
                //    config.Filters.Add(new AuthorizeFilter(requireAuthenticatedUserPolicy));
            });

            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //     .AddJwtBearer(options =>
            // {
            //     options.Authority = "https://localhost:3520";
            //     options.Audience = "orders";
            // });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders", Version = "v1" });
            });

            //    services.AddDbContext(Configuration);
            services.AddScoped<IOrdersRepository, OrdersRepository>();
            services.AddScoped<IOrderContext, OrderContext>();
            services.AddScoped<BasketCheckoutConsumer>();
            services.AddHttpClient();
            services.AddScoped<TokenValidationService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
            //    app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class ServiceCollectionExtensionMethods
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration config)
        {

            //services.AddMongoDb()
            // services.AddMongoDb(); 
            return services;
        }

        public static IServiceCollection AddClientMassTransit(this IServiceCollection services, IConfiguration config)
        {

            services.AddMassTransit(configuration =>
            {
                configuration.AddConsumer<BasketCheckoutConsumer>();
                configuration.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(config["EventBusAddress"]);
                    cfg.ReceiveEndpoint(config["BasketCheckoutQueue"], c =>
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
