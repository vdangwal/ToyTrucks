using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Infrastructure.Mail;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var server = configuration["ORDER_SERVER"];// ?? "(localdb)\\mssqllocaldb";
            var database = configuration["ORDER_DB"];// ?? "hess_catalog_db";
            var port = configuration["POSTGRES_PORT"];// ?? "5432"
            var user = configuration["ORDER_USER"];// ?? "marcus";
            var password = configuration["ORDER_PASSWORD"];// ?? "password";

            var connectionString = $"Host={server}; Port={port}; Database={database}; Username={user}; Password={password};";
            Console.WriteLine($"CONNECTION STRING Catalog: {connectionString}");
            services.AddDbContext<OrderContext>(options =>
               options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention()
                );

            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}