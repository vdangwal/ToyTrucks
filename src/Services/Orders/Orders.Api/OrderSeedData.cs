using Orders.Api.Entities;
using MongoDB.Driver;
using System.Collections.Generic;

using System.Linq;

namespace Orders.Api
{
    public class OrderSeedData
    {


        public static void SeedData(IMongoCollection<Order> ordersCollection)
        {
            bool orders = ordersCollection.Find(p => true).Any();
            if (!ordersCollection.Find(p => true).Any())
            {
                ordersCollection.InsertManyAsync(PopulateOrders());
            }
        }

        static IEnumerable<Order> PopulateOrders()
        {
            return new List<Order>()
            {
                new Order()
                {
                    UserName = "mq", FirstName = "Marcus", LastName = "Quigley", EmailAddress = "marcusQuigley@gmail.com", AddressLine = "Drogheda", Country = "Ireland", TotalPrice = 350
                }
            };
        }
    }
}