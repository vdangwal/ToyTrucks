using MongoDB.Driver;
using Basket.Api.Dtos;
using System.Collections.Generic;

namespace Basket.Api.DBContexts
{
    public interface IBasketContext
    {
        IMongoCollection<ShoppingCartDto> Baskets { get; }
        IMongoCollection<Dtos.Basket> Cart { get; }
        IMongoCollection<Dtos.BasketLine> CartLines { get; }
    }
}