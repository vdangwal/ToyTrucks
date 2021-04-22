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


            var user = configuration["ORDER_USER"];// ?? "marcus";
            var password = configuration["ORDER_PASSWORD"];// ?? "password";


            var connectionString = $"Data Source={server};Database={database};Trusted_Connection=true;MultipleActiveResultSets=true";


            services.AddDbContext<OrderContext>(options =>
               options.UseSqlServer(connectionString));


            services.AddTransient(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddTransient<IOrderRepository, OrderRepository>();

            services.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}