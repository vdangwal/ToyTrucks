using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Orders.Api.DBContexts;
using Orders.Api.Entities;

namespace Orders.Api
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {

            using (var dbContext = new OrderDbContext(
              serviceProvider.GetRequiredService<DbContextOptions<OrderDbContext>>()))
            {
                if (dbContext.Orders.Any())
                {
                    Console.WriteLine("Order database has already been seeded");
                    return;
                }
                Console.WriteLine("Seeding Order database");
                await PopulateTestData(dbContext);
            }
        }

        private static async Task PopulateTestData(OrderDbContext dbContext)
        {
            await PopulateOrders(dbContext);
        }

        private static async Task PopulateOrders(OrderDbContext dbContext)
        {
            dbContext.Orders.AddRange(GetPreconfiguredOrders());
            await dbContext.SaveChangesAsync();
        }
        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order() {UserName = "mq", FirstName = "Marcus", LastName = "Quigley", EmailAddress = "marcusQuigley@gmail.com", AddressLine = "Drogheda", Country = "Ireland", TotalPrice = 350 }
            };
        }
    }
}