using System;
using System.Threading.Tasks;
using Basket.Api.DBContexts;
using MongoDB.Driver;
using dtos = Basket.Api.Dtos;

namespace Basket.Api.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketContext _context;
        public BasketService(IBasketContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddBasket(dtos.Basket basket)
        {
            if (basket == null)
                throw new ArgumentNullException(nameof(basket));

            await _context.Cart.InsertOneAsync(basket);

        }



        public async Task<bool> BasketExists(Guid basketId)
        {
            if (basketId == Guid.Empty)
            {
                return false;
            }
            FilterDefinition<dtos.Basket> filter = Builders<dtos.Basket>.Filter.Eq(p => p.BasketId, basketId);
            return await _context.Cart
                                 .Find(filter)
                                 .AnyAsync();
        }

        public Task ClearBasket(Guid basketId)
        {
            throw new NotImplementedException();
        }

        public async Task<dtos.Basket> GetBasketById(Guid basketId)
        {
            if (basketId == Guid.Empty)
            {
                return null;
            }
            FilterDefinition<dtos.Basket> filter = Builders<dtos.Basket>.Filter.Eq(p => p.BasketId, basketId);
            return await _context.Cart
                                 .Find(filter)
                                 .FirstOrDefaultAsync();
            // var basket = await _cache.GetStringAsync(userName);
            // if (string.IsNullOrEmpty(basket))
            //     return null;
            // return JsonConvert.DeserializeObject<ShoppingCartDto>(basket);
        }

        public Task<bool> SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}