using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Extensions;
using Web.Models;
using Web.Models.Api;

namespace Web.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _client;
        private readonly Settings _settings;
        public BasketService(HttpClient client, Settings settings)
        {
            _client = client;
            _settings = settings;
        }


        public async Task<BasketLine> AddToBasket(Guid basketId, BasketLineForCreation basketLine)
        {
            if (basketId == Guid.Empty)
            {
                var basketResponse = await _client.PostAsJson("api/basket", new BasketForCreation { UserId = _settings.UserId });
                var basket = await basketResponse.ReadContentAs<Basket>();
                basketId = basket.BasketId;
            }
            var response = await _client.PostAsJson($"api/basket/{basketId}/basketlines", basketLine);
            return await response.ReadContentAs<BasketLine>();
        }

        public async Task<Basket> GetBasket(Guid basketId)
        {
            if (basketId == Guid.Empty)
            {
                return await CreateBasket();
            }
            var response = await _client.GetAsync($"api/basket/{basketId}");
            return await response.ReadContentAs<Basket>();
        }

        public Task<BasketForCheckout> Checkout(Guid basketId, BasketForCheckout basketForCheckout)
        {
            throw new NotImplementedException();
        }



        public Task<IEnumerable<BasketLine>> GetLinesForBasket(Guid basketId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLine(Guid basketId, Guid lineId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLine(Guid basketId, BasketLineForUpdate basketLineForUpdate)
        {
            throw new NotImplementedException();
        }

        public async Task<Basket> CreateBasket()
        {
            var basketResponse = await _client.PostAsJson("api/basket", new BasketForCreation { UserId = _settings.UserId });
            var basket = await basketResponse.ReadContentAs<Basket>();
            return basket;
        }
    }
}