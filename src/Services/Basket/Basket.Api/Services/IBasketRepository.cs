using System;
using System.Threading.Tasks;

using Basket.Api.Dtos;

namespace Basket.Api.Services
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string userName);
        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
        Task DeleteBasket(string userName);
    }
}