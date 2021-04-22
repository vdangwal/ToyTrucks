using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure.Persistence;
using Polly;
using System;

namespace Ordering.Api.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateAndSeedDatabase<TContext>(this IHost host, int? retries = 3) where TContext : DbContext
        {
            var policy = Policy.Handle<SqlException>().WaitAndRetry(retries.Value, times => TimeSpan.FromMilliseconds(times * 100));

            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<OrderContextSeed>>();
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
