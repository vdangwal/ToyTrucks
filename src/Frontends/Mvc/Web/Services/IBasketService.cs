using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models.Api;

namespace Web.Services
{
    public interface IBasketService
    {
        //Task<BasketLine> AddToBasket(Guid basketId, BasketLineForCreation basketLine);
        //Task<IEnumerable<BasketLine>> GetLinesForBasket(Guid basketId);
        // /Task<Basket> GetBasket(Guid basketId);
        Task<CustomerBasket> GetBasket(string userId);
        Task<CustomerBasket> AddLine(string basketId, BasketItem basketItem);
        Task DeleteBasket(string userId);
        //Task<Basket> CreateBasket();
        Task UpdateLine(string basketId, string lineId, int quantity);
        Task<CustomerBasket> RemoveLine(string basketId, string lineId);
        Task<BasketForCheckout> Checkout(Guid basketId, BasketForCheckout basketForCheckout);
    }
}