using MongoDB.Driver;
using Orders.Api.Models;
namespace Orders.Api.DBContexts
{
    public interface IOrderContext
    {
        IMongoCollection<OrderDto> Orders { get; }
    }
}