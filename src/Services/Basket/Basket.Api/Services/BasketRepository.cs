using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Api.Services
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _cache;
        public BasketRepository(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task DeleteBasket(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));
            await _cache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));
            var basket = await _cache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
                return null;
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            if (basket == null || string.IsNullOrEmpty(basket.UserName))
                throw new ArgumentNullException(nameof(basket));
            await _cache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            return await GetBasket(basket.UserName);
        }
    }
}