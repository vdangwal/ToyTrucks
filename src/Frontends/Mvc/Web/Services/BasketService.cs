using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<BasketService> _logger;

        public BasketService(HttpClient client, Settings settings, IHttpContextAccessor httpContextAccessor, ILogger<BasketService> logger)
        {
            _client = client;
            _settings = settings;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }


        // public async Task<BasketLine> AddToBasket(Guid basketId, BasketLineForCreation basketLine)
        // {
        //     if (basketId == Guid.Empty)
        //     {
        //         var basketResponse = await _client.PostAsJson("api/basket", new BasketForCreation { UserId = _settings.UserId });
        //         var basket = await basketResponse.ReadContentAs<Basket>();
        //         basketId = basket.BasketId;
        //     }
        //     var response = await _client.PostAsJson($"api/baskets/v2/{basketId}/basketlines", basketLine);
        //     return await response.ReadContentAs<BasketLine>();
        // }

        public async Task<CustomerBasket> GetBasket(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            var response = await _client.GetAsync($"api/v1/basket/{id}");

            return await response.ReadContentAs<CustomerBasket>();
        }

        public async Task<CustomerBasket> AddLine(string basketId, BasketItem basketItem)
        {
            if (string.IsNullOrWhiteSpace(basketId))
            {
                throw new ArgumentNullException(nameof(basketId));
            }
            if (basketItem == null)
            {
                throw new ArgumentNullException(nameof(basketItem));
            }

            var customerBasket = await GetBasket(basketId);
            if (customerBasket == null)
            {
                throw new ArgumentNullException($"customer basket does not exist for basketid of{basketId}");
            }
            basketItem.Id = Guid.NewGuid().ToString();

            customerBasket.Items.Add(basketItem);


            var response = await _client.PostAsJson($"api/v1/basket", customerBasket);

            return await response.ReadContentAs<CustomerBasket>();
        }

        public async Task DeleteBasket(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            await _client.DeleteAsync($"api/v1/basket/{userId}");
        }

        public Task<BasketForCheckout> Checkout(Guid basketId, BasketForCheckout basketForCheckout)
        {
            throw new NotImplementedException();

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

        public async Task<CustomerBasket> RemoveLine(string basketId, string lineId)
        {

            CustomerBasket basket = await GetBasket(basketId);
            var basketItem = basket?.Items?.FirstOrDefault(bi => bi.Id == lineId);
            if (basketItem != null)
            {
                basket.Items.Remove(basketItem);
                var response = await _client.PostAsJson($"api/v1/basket", basket);
                return await response.ReadContentAs<CustomerBasket>();
            }
            else
            {
                _logger.LogWarning($"No basket item found with id of {lineId}");
                return basket;
            }
        }

        public async Task UpdateLine(string basketId, string lineId, int quantity)
        {
            CustomerBasket basket = await GetBasket(basketId);
            var basketItem = basket?.Items?.FirstOrDefault(bi => bi.Id == lineId);
            if (basketItem != null)
            {
                basketItem.Quantity = quantity;
                var response = await _client.PostAsJson($"api/v1/basket", basket);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"{response.ReasonPhrase}");
                }
                //                return await response.ReadContentAs<CustomerBasket>();
            }
            else
            {
                _logger.LogWarning($"No basket item found with id of {lineId}");
                //              return basket;
            }
        }
    }
}