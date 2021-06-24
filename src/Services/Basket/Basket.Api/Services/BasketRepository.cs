using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Basket.Api.Dtos;
using Basket.Api.DBContexts;
using MongoDB.Bson;
using MongoDB.Driver;
using EventBus.Messages.Events;

namespace Basket.Api.Services
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketContext _context;
        public BasketRepository(IBasketContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private async Task<bool> BasketExists(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));
            FilterDefinition<ShoppingCartDto> filter = Builders<ShoppingCartDto>.Filter.Eq(p => p.UserName, userName);

            return await _context.Baskets
                                 .Find(filter)
                                 .AnyAsync();
        }

        public async Task<ShoppingCartDto> GetBasket(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));

            FilterDefinition<ShoppingCartDto> filter = Builders<ShoppingCartDto>.Filter.Eq(p => p.UserName, userName);
            return await _context.Baskets
                                 .Find(filter)
                                 .FirstOrDefaultAsync();
            // var basket = await _cache.GetStringAsync(userName);
            // if (string.IsNullOrEmpty(basket))
            //     return null;
            // return JsonConvert.DeserializeObject<ShoppingCartDto>(basket);
        }

        public async Task<ShoppingCartDto> UpdateBasket(ShoppingCartDto basket)
        {
            if (basket == null || string.IsNullOrEmpty(basket.UserName))
                throw new ArgumentNullException(nameof(basket));
            if (!await BasketExists(basket.UserName))
            {
                await _context.Baskets.InsertOneAsync(basket);
            }
            else
            {
                var updateResult = await _context.Baskets
                                                 .ReplaceOneAsync(filter: g => g.UserName == basket.UserName, replacement: basket);
            }

            return await GetBasket(basket.UserName);
        }

        public async Task<bool> DeleteBasket(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));
            if (await BasketExists(userName))
            {

                FilterDefinition<ShoppingCartDto> filter = Builders<ShoppingCartDto>.Filter.Eq(p => p.UserName, userName);

                DeleteResult deleteResult = await _context.Baskets
                                                          .DeleteOneAsync(filter);

                return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
            }
            return false;
        }

        public async Task<IList<ShoppingCartDto>> InformOfUpdatedInventory(UpdatedInventory inventoryDetails)
        {
            if (inventoryDetails == null)
                throw new ArgumentNullException(nameof(inventoryDetails));

            //TODO need to return only subitems if I can instead of complete basket
            var baskets = Builders<ShoppingCartDto>.Filter.Eq("Items.ProductId", inventoryDetails.TruckId);

            var results = await _context.Baskets.Find(baskets).ToListAsync();
            foreach (var cart in results)
            {
                foreach (var item in cart.Items)
                {
                    if (item.ProductId == inventoryDetails.TruckId)
                        item.OutOfStock = true;
                }
            }
            return results;
            //TODO NEED TO UPDATE THE BASKET ITEM


        }
    }
}