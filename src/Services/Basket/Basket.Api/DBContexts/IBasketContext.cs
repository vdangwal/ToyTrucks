using MongoDB.Driver;
using Basket.Api.Dtos;
namespace Basket.Api.DBContexts
{
    public interface IBasketContext
    {
        IMongoCollection<ShoppingCartDto> Baskets { get; }
    }
}