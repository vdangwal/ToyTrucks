//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;
using System;

namespace ToyTrucks.Discount.Grpc.Extensions
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

                policy.Execute(() =>
                {
                    SeedData.Initialize(config).Wait();
                    logger.LogInformation("Discount database seeded");
                    Console.WriteLine("Discount database seeded");

                });
                return host;
            }
        }
    }
}
