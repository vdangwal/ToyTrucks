using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models.Api;

namespace Web.Services
{
    public interface IBasketService
    {
        Task<BasketLine> AddToBasket(Guid basketId, BasketLineForCreation basketLine);
        Task<IEnumerable<BasketLine>> GetLinesForBasket(Guid basketId);
        Task<Basket> GetBasket(Guid basketId);
        Task<Basket> CreateBasket();
        Task UpdateLine(Guid basketId, BasketLineForUpdate basketLineForUpdate);
        Task RemoveLine(Guid basketId, Guid lineId);
        Task<BasketForCheckout> Checkout(Guid basketId, BasketForCheckout basketForCheckout);
    }
}