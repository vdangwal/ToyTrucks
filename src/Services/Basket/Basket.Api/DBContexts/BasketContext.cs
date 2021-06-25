using MongoDB.Driver;
using Basket.Api.Dtos;
using Microsoft.Extensions.Configuration;
namespace Basket.Api.DBContexts
{
    public class BasketContext : IBasketContext
    {

        public BasketContext(IConfiguration configuration)
        {
            var server = configuration["MONGO_SERVER"];// ?? "localhost";

            var port = configuration["MONGO_PORT"];// ?? "5432";
            var database = configuration["MONGO_DB"] ?? "BasketDb";


            var collection = configuration["MONGO_COLLECTION"];// ?? "Baskets";
            var client = new MongoClient($"mongodb://{server}:{port}");

            var db = client.GetDatabase(database);
            Baskets = db.GetCollection<ShoppingCartDto>(collection);

            // OrderSeedData.SeedData(Baskets);

            //docker run -d -p 27017:27017 --name order-mongo   mongo
            //following are mongo cmds
            //mongo
            //show dbs
            //show collections
            //use Baskets //creates anew db
            //db.createCollection('Orders')
            //db.Orders.insertMany([{"name","value", "name2","value2"}])
            //db.Orders.remove({});
            //db.Baskets.find({}).pretty();
        }
        public IMongoCollection<ShoppingCartDto> Baskets { get; }
    }
}