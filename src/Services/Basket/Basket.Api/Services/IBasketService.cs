using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dtos = Basket.Api.Dtos;

namespace Basket.Api.Services
{
    public interface IBasketService
    {
        Task<bool> BasketExists(Guid basketId);

        Task<dtos.Basket> GetBasketById(Guid basketId);


        Task AddBasket(dtos.Basket basket);

        Task<bool> SaveChanges();

        Task ClearBasket(Guid basketId);
    }
}