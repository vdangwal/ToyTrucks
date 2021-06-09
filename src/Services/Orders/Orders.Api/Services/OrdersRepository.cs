using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Orders.Api.DBContexts;
using Orders.Api.Models;
namespace Orders.Api.Services
{
    public class OrdersRepository : IOrdersRepository
    {

        private readonly IOrderContext _context;

        public OrdersRepository(IOrderContext dbContext)
        {
            _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        private async Task<bool> OrderExists(ObjectId orderId)
        {
            FilterDefinition<OrderDto> filter = Builders<OrderDto>.Filter.Eq(p => p.Id, orderId);
            return await _context.Orders
                                 .Find(filter)
                                 .AnyAsync();
        }

        public async Task<OrderDto> GetByOrderIdAsync(string id)
        {
            var orderId = new ObjectId(id);
            if (!await OrderExists(orderId))
                throw new Exception($"Order doesnt exist with id of {id}");

            //   FilterDefinition<Order> filter = Builders<Order>.Filter.ElemMatch(p => p.Id, id);
            FilterDefinition<OrderDto> filter = Builders<OrderDto>.Filter.Eq(p => p.Id, orderId);

            return await _context.Orders
                                 .Find(filter)
                                 .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<OrderDto>> GetOrdersAsync()
        {
            return await _context.Orders
                                 .Find(_ => true)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByUserName(string userName)
        {
            FilterDefinition<OrderDto> filter = Builders<OrderDto>.Filter.Eq(p => p.UserName, userName);
            return await _context.Orders
                                 .Find(filter)
                                 .ToListAsync();
        }

        public async Task<OrderDto> AddOrderAsync(OrderDto order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            // var newEntity = await _context.Orders.AddAsync(order);
            // await _context.SaveChangesAsync();
            // return newEntity.Entity;

            await _context.Orders.InsertOneAsync(order);
            return await Task.FromResult(order);
        }

        public async Task<bool> UpdateOrderAsync(OrderDto order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            if (!await OrderExists(order.Id))
                throw new Exception($"Order doesnt exist with id of {order.Id}");

            var updateResult = await _context.Orders
                                             .ReplaceOneAsync(filter: g => g.Id == order.Id, replacement: order);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;

            // _context.Entry(order).State = EntityState.Modified;
            // await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteOrderAsync(string id)
        {
            // var order = await GetByOrderIdAsync(id);
            // if (order == null)
            //     throw new ArgumentNullException(nameof(order));
            // _context.Orders.Remove(order);
            // await _context.SaveChangesAsync();

            FilterDefinition<OrderDto> filter = Builders<OrderDto>.Filter.Eq(p => p.Id, new ObjectId(id));

            DeleteResult deleteResult = await _context.Orders
                                                      .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}