using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToyTrucks.Basket.Api.Dtos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
namespace ToyTrucks.Basket.Api.Services
{
    public class RedisBasketRepository : IBasketRepository
    {
        private readonly ILogger<RedisBasketRepository> _logger;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisBasketRepository(ILoggerFactory loggerFactory, ConnectionMultiplexer redis)
        {
            _logger = loggerFactory.CreateLogger<RedisBasketRepository>();
            _redis = redis;
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }

        public async Task<CustomerBasket> GetBasketAsync(string userId)
        {
            var data = await _database.StringGetAsync(userId);

            if (data.IsNullOrEmpty)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<CustomerBasket>(data);
        }

        public IEnumerable<string> GetUsers()
        {
            var server = GetServer();
            var data = server.Keys(pattern: "*").ToArray();

            return data?.Select(k => k.ToString());
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var created = await _database.StringSetAsync(basket.UserId, JsonConvert.SerializeObject(basket));

            if (!created)
            {
                _logger.LogInformation("Problem occur persisting the item.");
                return null;
            }

            _logger.LogInformation("Basket item persisted succesfully.");

            return await GetBasketAsync(basket.UserId);
        }

        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }

        public async Task<IEnumerable<CustomerBasket>> GetBasketsWithTruck(Guid truckId)
        {
            var userIds = GetUsers();
            var basketsWithTruckId = new List<CustomerBasket>();
            foreach (var userId in userIds)
            {
                var basket = await GetBasketAsync(userId);
                if (basket.Items.Any(item => item.TruckId == truckId))
                    basketsWithTruckId.Add(basket);
            }
            return basketsWithTruckId;
        }
    }
}