using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventBus.Messages.Events;
using Basket.Api.Dtos;

namespace Basket.Api.Services
{
    public interface IBasketRepository
    {
        Task<ShoppingCartDto> GetBasket(string userName);
        Task<ShoppingCartDto> UpdateBasket(ShoppingCartDto basket);
        Task<bool> DeleteBasket(string userName);
        Task<IList<ShoppingCartDto>> InformOfUpdatedInventory(UpdatedInventory inventoryDetails);
    }
}