using MongoDB.Driver;
using Orders.Api.Entities;
namespace Orders.Api.DBContexts
{
    public interface IOrderContext
    {
        IMongoCollection<Order> Orders { get; }
    }
}