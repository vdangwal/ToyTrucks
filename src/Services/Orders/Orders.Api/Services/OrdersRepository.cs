using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Orders.Api.DBContexts;
using Orders.Api.Entities;
namespace Orders.Api.Services
{
    public class OrdersRepository : IOrdersRepository
    {

        private readonly OrderDbContext _dbContext;

        public OrdersRepository(OrderDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        private async Task<bool> OrderExists(int orderId)
        {
            return await _dbContext.Orders.AnyAsync(o => o.Id == orderId);
        }

        public async Task<Order> GetByOrderIdAsync(int id)
        {
            if (!await OrderExists(id))
                throw new Exception($"Order doesnt exist with id of {id}");
            return await _dbContext.Orders
                         .Where(o => o.Id == id)
                         .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Order>> GetOrdersAsync()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            return await _dbContext.Orders
                         .Where(o => o.UserName == userName)
                         .ToListAsync();
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            var newEntity = await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return newEntity.Entity;
        }

        public async Task UpdateOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            if (!await OrderExists(order.Id))
                throw new Exception($"Order doesnt exist with id of {order.Id}");
            _dbContext.Entry(order).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteOrderAsync(int id)
        {
            var order = await GetByOrderIdAsync(id);
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
        }
    }
}