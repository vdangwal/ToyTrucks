using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Extensions;
using Web.Models.Api;

namespace Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _client;

        public OrderService(HttpClient client)
        {
            _client = client;
        }
        public OrderService()
        {

        }
        public async Task<IEnumerable<Order>> GetOrdersForUser(Guid userId)
        {
            var response = await _client.GetAsync($"api/orders/user/{userId}");
            return await response.ReadContentAs<IEnumerable<Order>>();
        }
        public async Task<Order> GetOrderDetails(Guid orderId)
        {
            var response = await _client.GetAsync($"api/orders/{orderId}");
            var order = await response.ReadContentAs<Order>();
            return order;
        }
    }
}