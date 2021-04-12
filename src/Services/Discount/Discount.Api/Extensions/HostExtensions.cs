//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;
using System;

namespace Discount.Api.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateAndSeedDatabase<TContext>(this IHost host, int retries = 3) //where T : DbContext
        {

            var policy = Polly.Policy.Handle<Npgsql.NpgsqlException>().WaitAndRetry(retries, times => TimeSpan.FromMilliseconds(times * 100));
            using (var scope = host.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var server = config["POSTGRES_SERVER"];
                var port = config["POSTGRES_PORT"];
                var database = config["POSTGRES_DB"];
                var user = config["POSTGRES_USER"];
                var password = config["POSTGRES_PASSWORD"];
                var connectionString = $"Host={server}; Port={port}; Database={database}; Username={user}; Password={password};";

                policy.Execute(() =>
                {
                    SeedData.Initialize(connectionString).Wait();
                    // logger.LogInformation("Discount database seeded");
                    //Console.WriteLine("Discount database seeded");

                });
                return host;
            }
        }
    }
}
