using System;
using System.Threading.Tasks;

using Basket.Api.Dtos;

namespace Basket.Api.Services
{
    public interface IBasketRepository
    {
        Task<ShoppingCartDto> GetBasket(string userName);
        Task<ShoppingCartDto> UpdateBasket(ShoppingCartDto basket);
        Task DeleteBasket(string userName);
    }
}