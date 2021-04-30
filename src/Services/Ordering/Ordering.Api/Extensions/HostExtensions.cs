//using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Ordering.Infrastructure.Persistence;
using Polly;
using System;

namespace Ordering.Api.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateAndSeedDatabase<TContext>(this IHost host, int? retries = 3) where TContext : DbContext
        {
            var policy = Policy.Handle<Npgsql.NpgsqlException>().WaitAndRetry(retries.Value, times => TimeSpan.FromMilliseconds(times * 100));

            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<OrderContextSeed>>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                //logDbConnectionstring(logger, config);
                var server = configuration["ORDER_SERVER"];// ?? "(localdb)\\mssqllocaldb";
                var database = configuration["ORDER_DB"];// ?? "orderdb";
                var port = configuration["POSTGRES_PORT"];// ?? "5432"
                var user = configuration["ORDER_USER"];// ?? "marcus";
                var password = configuration["ORDER_PASSWORD"];// ?? "password";

                var connectionString = $"Host={server}; Port={port}; Database={database}; Username={user}; Password={password};";

                logger.LogInformation($"db conn string: {connectionString}");
                policy.Execute(() =>
                {
                    context.Database.Migrate();
                    context.Database.EnsureCreated();

                    OrderContextSeed.SeedAsync(context as OrderContext, logger)
                            .Wait();
                });
            }
            return host;
        }
    }
}
