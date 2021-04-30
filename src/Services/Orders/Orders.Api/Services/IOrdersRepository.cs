using System.Collections.Generic;
using System.Threading.Tasks;
using Orders.Api.Entities;
namespace Orders.Api.Services
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
        Task<IReadOnlyList<Order>> GetOrdersAsync();
        Task<Order> GetByOrderIdAsync(int id);
        Task<Order> AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
    }
}