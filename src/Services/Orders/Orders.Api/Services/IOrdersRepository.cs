using System.Collections.Generic;
using System.Threading.Tasks;
using Orders.Api.Entities;
namespace Orders.Api.Services
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
        Task<IReadOnlyList<Order>> GetOrdersAsync();
        Task<Order> GetByOrderIdAsync(string id);
        Task<Order> AddOrderAsync(Order order);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(string id);
    }
}