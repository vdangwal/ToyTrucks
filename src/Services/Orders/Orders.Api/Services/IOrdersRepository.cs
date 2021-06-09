using System.Collections.Generic;
using System.Threading.Tasks;
//using Orders.Api.Entities;
using Orders.Api.Models;

namespace Orders.Api.Services
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<OrderDto>> GetOrdersByUserName(string userName);
        Task<IReadOnlyList<OrderDto>> GetOrdersAsync();
        Task<OrderDto> GetByOrderIdAsync(string id);
        Task<OrderDto> AddOrderAsync(OrderDto order);
        Task<bool> UpdateOrderAsync(OrderDto order);
        Task<bool> DeleteOrderAsync(string id);
    }
}