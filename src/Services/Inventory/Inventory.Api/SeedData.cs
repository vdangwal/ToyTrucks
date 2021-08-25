using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Inventory.Api.DbContexts;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Inventory.Api
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {

            using (var dbContext = new InventoryDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<InventoryDbContext>>()))
            {
                if (dbContext.TruckInventory.Any())
                {
                    Console.WriteLine("TruckInventory database has already been seeded");
                    return;   // DB has been seeded
                }
                Console.WriteLine("Seeding TruckInventory database");
                await PopulateTruckInventorys(dbContext);

            }
        }

        private static async Task PopulateTruckInventorys(InventoryDbContext dbContext)
        {
            string truckInventorySql = "insert into truck_inventory(truck_id, truck_name, quantity) values " +
            " ('9f33f915-7273-48d0-8a42-9cfbf9023a61', 'Hess 1986 Fire Truck', 2), " +
            " ('b0983d45-f0a3-4161-8683-7ff81b41880a', 'Hess 1987 18 Wheeler Bank', 7)," +
            " ('705af6db-0660-4642-8b08-f0a9aeacb326', 'Hess 1988 Truck and Racer', 5), " +
            " ('bc08fa9a-4a1b-4ee8-9594-1fe1fa2d9886', 'Hess 1989 Fire Truck Bank', 2), " +
            " ('f27e6639-6237-4d92-a284-d078e54c28d5', 'Hess 1990 Tanker Truck',  2), " +
            " ('7c19baad-a97c-4cd7-adcf-57fef037576d', 'Hess 1991 Truck and Racer', 2), " +
            " ('e0a8eac6-ec49-48c0-ad71-266c985d3d77', 'Hess 1992 18 Wheeler and Racer', 3), " +
            " ('087d995f-fee6-4d8e-85e4-87510eb7f98b', 'Hess 1994 Rescue Truck', 22), " +
            " ('209c0ae5-b382-410f-952f-0f0038af465d', 'Hess 1995 Truck and Helicopter', 22), " +
            " ('6cb05a0a-45ca-476f-9949-5792f846d442', 'Hess 1996 Emergency Truck',  20), " +
            " ('0bbef086-0a98-4303-a193-f6016262e911', 'Hess 1997 Truck and Racers', 17), " +
            " ('11a0e508-0d7d-4111-8162-82e8e90ceaa3', 'Hess 1998 Recreation Van', 2), " +
            " ('2ebe373a-239b-4326-bcaf-525ffce8a130', 'Hess 1999 Truck and Space Shuttle with Satellite',18), " +
            " ('24de1ae0-7221-4c3d-a709-797136d8639f', 'Hess 2000 Fire Truck',2), " +
            " ('17b11bfe-002b-4f94-b3f3-af71ebf43848', 'Hess 2001 Helicopter With Motorcycle and Cruiser', 8), " +
            " ('101a65d5-50b4-40fd-aa39-80286c7dfa24', 'Hess 2002 Truck and Airplane', 9), " +
            " ('00827712-d0ab-4434-b62f-2bfeecbfce1d', 'Hess 2003 18 Wheeler Truck and Race Cars', 14), " +
            " ('7b91314b-d07a-42c8-9182-232e93f484f7', 'Hess 2004 Sport Utility Vehicle & Motorcycles',1), " +
            " ('6ce26afe-8f44-4db6-bcb4-7736d345d862', 'Hess 2005 Emergency Truck with Rescue Vehicle', 1), " +
            " ('e09e6602-fe5b-4b78-8fa4-ec8a93cdd675', 'Hess 2006 Truck and Helicopter',8), " +
            " ('25340508-eda6-487b-af69-bd85b96347a3', 'Hess 2008 Toy Truck & Front End Loader', 1), " +
            " ('5d8d0803-d024-4de9-9a85-bc83ef115d44', 'Hess 2009 Toy Race Car and Racer',  7), " +
            " ('95ca027a-a18d-4007-8d92-ed92e862cea8', 'Hess 2011 Toy Truck and Racer',4), " +
            " ('028b1e54-6008-47c1-a7ef-6ba5ae5540a1', 'Hess 2012 Helicopter and Rescue SUV', 1), " +
            " ('a4dec62a-ae82-4e4f-965a-b5779e799ac2', 'Hess 2013 Truck & Tractor', 1), " +
            " ('cb8433fc-fff0-48c8-a2f9-b92cb66582d5', 'Hess 2014 Toy Truck & Space Cruiser With Scout',1); ";

            await dbContext.Database.ExecuteSqlRawAsync(truckInventorySql);

        }
    }
}
