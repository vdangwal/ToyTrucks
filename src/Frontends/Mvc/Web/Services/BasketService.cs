using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Web.Extensions;
using Web.Models;
using Web.Models.Api;

namespace Web.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Settings _settings;
        public BasketService(HttpClient client, Settings settings, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _settings = settings;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<BasketLine> AddToBasket(Guid basketId, BasketLineForCreation basketLine)
        {
            if (basketId == Guid.Empty)
            {
                var basketResponse = await _client.PostAsJson("api/basket", new BasketForCreation { UserId = _settings.UserId });
                var basket = await basketResponse.ReadContentAs<Basket>();
                basketId = basket.BasketId;
            }
            var response = await _client.PostAsJson($"api/baskets/v2/{basketId}/basketlines", basketLine);
            return await response.ReadContentAs<BasketLine>();
        }

        public async Task<Basket> GetBasket(Guid basketId)
        {
            if (basketId == Guid.Empty)
            {
                return await CreateBasket();
            }
            var response = await _client.GetAsync($"api/baskets/v2/{basketId}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return await CreateBasket();
            }
            return await response.ReadContentAs<Basket>();
        }

        public Task<BasketForCheckout> Checkout(Guid basketId, BasketForCheckout basketForCheckout)
        {
            throw new NotImplementedException();

        }

        public async Task<IEnumerable<BasketLine>> GetLinesForBasket(Guid basketId)
        {
            if (basketId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(basketId));
            }
            var response = await _client.GetAsync($"api/baskets/v2/{basketId}/basketlines");
            return await response.ReadContentAs<BasketLine[]>();
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
            var basketResponse = await _client.PostAsJson("api/baskets/v2", new BasketForCreation { UserId = _settings.UserId });
            var basket = await basketResponse.ReadContentAs<Basket>();
            await CreateBasketCookie(basket.BasketId);
            return basket;
        }

        private Task CreateBasketCookie(Guid basketId)
        {
            if (basketId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(basketId));
            }
            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(21);
            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                _settings.BasketIdCookieName, basketId.ToString(), cookieOptions);
            return Task.CompletedTask;
        }


    }
}