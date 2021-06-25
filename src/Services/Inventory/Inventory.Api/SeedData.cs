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
            string truckInventorySql = "insert into truck_inventory(truck_name, quantity) values " +
                "('Hess 1986 Fire Truck', 2), " +
            "  ('Hess 1987 18 Wheeler Bank', 7)," +
            "  ('Hess 1988 Truck and Racer', 5), " +
            " ('Hess 1989 Fire Truck Bank', 2), " +
            " ('Hess 1990 Tanker Truck',  2), " +
            " ('Hess 1991 Truck and Racer', 2), " +
            " ('Hess 1992 18 Wheeler and Racer', 3), " +
            " ('Hess 1994 Rescue Truck', 22), " +
            " ('Hess 1995 Truck and Helicopter', 22), " +
            " ('Hess 1996 Emergency Truck',  20), " +
            " ('Hess 1997 Truck and Racers', 17), " +
            " ('Hess 1998 Recreation Van', 2), " +
            " ('Hess 1999 Truck and Space Shuttle with Satellite',18), " +
            " ('Hess 2000 Fire Truck',2), " +
            " ('Hess 2001 Helicopter With Motorcycle and Cruiser', 8), " +
            " ('Hess 2002 Truck and Airplane', 9), " +
            " ('Hess 2003 18 Wheeler Truck and Race Cars', 14), " +
            " ('Hess 2004 Sport Utility Vehicle & Motorcycles',1), " +
            " ('Hess 2005 Emergency Truck with Rescue Vehicle', 1), " +
            " ('Hess 2006 Truck and Helicopter',8), " +
            " ('Hess 2008 Toy Truck & Front End Loader', 1), " +
            " ('Hess 2009 Toy Race Car and Racer',  7), " +
            " ('Hess 2011 Toy Truck and Racer',4), " +
           " ('Hess 2012 Helicopter and Rescue SUV', 1), " +
           " ('Hess 2013 Truck & Tractor', 1), " +
           " ('Hess 2014 Toy Truck & Space Cruiser With Scout',1); ";

            await dbContext.Database.ExecuteSqlRawAsync(truckInventorySql);

        }
    }
}
