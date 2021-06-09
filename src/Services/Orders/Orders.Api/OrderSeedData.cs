using Orders.Api.Models;
using MongoDB.Driver;
using System.Collections.Generic;

using System.Linq;

namespace Orders.Api
{
    public class OrderSeedData
    {


        public static void SeedData(IMongoCollection<OrderDto> ordersCollection)
        {
            bool orders = ordersCollection.Find(p => true).Any();
            if (!ordersCollection.Find(p => true).Any())
            {
                ordersCollection.InsertManyAsync(PopulateOrders());
            }
        }

        static IEnumerable<OrderDto> PopulateOrders()
        {
            var basket = new OrderBasket();
            basket.Items = new List<OrderItemDto>(){
                new OrderItemDto(){ProductId="3", ProductName="gun", Price = 23.20m, Quantity=2},
                new OrderItemDto(){ProductId="33", ProductName="big gun", Price = 232.20m, Quantity=1}
                };

            return new List<OrderDto>()
            {
                new OrderDto()
                {
                    UserName = "mq", FirstName = "Marcus", LastName = "Quigley", EmailAddress = "marcusQuigley@gmail.com",
                    AddressLine = "Drogheda", Country = "Ireland", TotalPrice = 350,
                    OrderItems = basket
                }
            };
        }
    }
}

