using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Catalog.Api.DbContexts;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Catalog.Api
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {

            using (var dbContext = new CatalogDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<CatalogDbContext>>()))
            {
                if (dbContext.Trucks.Any())
                {
                    Console.WriteLine("Catalog database has already been seeded");
                    return;   // DB has been seeded
                }
                Console.WriteLine("Seeding Catalog database");
                await PopulateTestData(dbContext);

            }
        }

        private static async Task PopulateTestData(CatalogDbContext dbContext)
        {
            await PopulateTrucks(dbContext);
            await PopulateCategories(dbContext);
            if (dbContext.Trucks.Any())
            {
                if (dbContext.Categories.Any())
                {
                    await PopulateTruckCategories(dbContext);
                }
                await PopulatePhotos(dbContext);
            }
        }

        private static async Task PopulateTrucks(CatalogDbContext dbContext)
        {
            string truckSql = "insert into trucks(truck_id, name, year, description, price,  default_photo_path) values " +
                " ('9f33f915-7273-48d0-8a42-9cfbf9023a61', 'Hess 1986 Fire Truck', 1986, 'Chrome grill version. It has working headlights, taillights, and a flashing red light on the cab of the fire truck', 100.99,  '1986_1.jpg'), " +
                " ('b0983d45-f0a3-4161-8683-7ff81b41880a', 'Hess 1987 18 Wheeler Bank', 1987, 'It has clearance and running lights in addition to the headlights and taillights', 11.99, '1987_1.jpg')," +
                " ('705af6db-0660-4642-8b08-f0a9aeacb326', 'Hess 1988 Truck and Racer', 1988, 'It has marker lights, headlights, and taillights', 11.99,  '1988_1.jpg'), " +
                " ('bc08fa9a-4a1b-4ee8-9594-1fe1fa2d9886', 'Hess 1989 Fire Truck Bank', 1989, 'It has headlights, taillights and emergency flashing lights in addition to a unique dual tone siren', 11.99,  '1989_1.jpg'), " +
                " ('f27e6639-6237-4d92-a284-d078e54c28d5', 'Hess 1990 Tanker Truck', 1990, 'It has 34 lights, lighted Hess logos, a backup alert along with a horn', 11.99,  '1990_1.jpg'), " +
                " ('7c19baad-a97c-4cd7-adcf-57fef037576d', 'Hess 1991 Truck and Racer', 1991, 'It has 27 different red, white, and yellow lights', 11.99,  '1991_1.jpg'), " +
                " ('e0a8eac6-ec49-48c0-ad71-266c985d3d77', 'Hess 1992 18 Wheeler and Racer', 1992, 'Lights...', 11.99,  '1992_1.jpg'), " +
                " ('087d995f-fee6-4d8e-85e4-87510eb7f98b', 'Hess 1994 Rescue Truck', 1994, 'Lights... . Sounds include a back up alert, truck horn, and an emergency siren ', 11.99,  '1994_1.jpg'), " +
                " ('209c0ae5-b382-410f-952f-0f0038af465d', 'Hess 1995 Truck and Helicopter', 1995, 'Lights... Chopper has a searchlight, and flashing beacon lights ', 11.99, '1995_1.jpg'), " +
                " ('6cb05a0a-45ca-476f-9949-5792f846d442', 'Hess 1996 Emergency Truck', 1996, 'It has a searchlight, flashers, emergency lights, a backup alert along with a siren', 11.99,  '1996_1.jpg'), " +
                " ('0bbef086-0a98-4303-a193-f6016262e911', 'Hess 1997 Truck and Racers', 1997, 'Lights... Racers have working headlights and taillights', 11.99,  '1997_1.jpg'), " +
                " ('11a0e508-0d7d-4111-8162-82e8e90ceaa3', 'Hess 1998 Recreation Van', 1998, 'Rv has headlights, taillights, and market lights.', 11.99,  '1998_1.jpg'), " +
                " ('2ebe373a-239b-4326-bcaf-525ffce8a130', 'Hess 1999 Truck and Space Shuttle with Satellite', 1999, 'Truck has working headlights, taillights, and running lights. Shuttle has lights, sounds and a satellite with solar panels', 11.99,  '1999_1.jpg'), " +
                " ('24de1ae0-7221-4c3d-a709-797136d8639f', 'Hess 2000 Fire Truck', 2000, 'It has  working headlights, taillights and emergency flashing lights, a backup alert along with a horn', 11.99,  '2000_1.jpg'), " +
                " ('17b11bfe-002b-4f94-b3f3-af71ebf43848', 'Hess 2001 Helicopter With Motorcycle and Cruiser', 2001, 'Helicopter that has internal, external lights and emergency flasher', 11.99,  '2001_1.jpg'), " +
                " ('101a65d5-50b4-40fd-aa39-80286c7dfa24', 'Hess 2002 Truck and Airplane', 2002, ' The truck has front and rear lights', 11.99,  '2002_1.jpg'), " +
                " ('00827712-d0ab-4434-b62f-2bfeecbfce1d', 'Hess 2003 18 Wheeler Truck and Race Cars', 2003, 'The truck has running lights, front and rear lights, an internally lit trailer.', 11.99,  '2003_1.jpg'), " +
                " ('7b91314b-d07a-42c8-9182-232e93f484f7', 'Hess 2004 Sport Utility Vehicle & Motorcycles', 2004, 'Lights... . Bikes have working lights', 11.99, '2004_1.jpg'), " +
                " ('6ce26afe-8f44-4db6-bcb4-7736d345d862', 'Hess 2005 Emergency Truck with Rescue Vehicle', 2005, 'Truck has front and rear lights, emergency flashing red lights, and three different emergency sirens sounds.', 11.99,  '2005_1.jpg'), " +
                " ('e09e6602-fe5b-4b78-8fa4-ec8a93cdd675', 'Hess 2006 Truck and Helicopter', 2006, 'Truck has front and rear lights and landing lights as well as 31 separate working lights on the trailer.', 11.99,  '2006_1.jpg'), " +
                " ('25340508-eda6-487b-af69-bd85b96347a3', 'Hess 2008 Toy Truck & Front End Loader', 2008, 'Truck has headlights, taillights, and running lights. it also has a real-sounding ignition, back up alert, hydraulic arm sound, horn', 11.99,  '2008_1.jpg'), " +
                " ('5d8d0803-d024-4de9-9a85-bc83ef115d44', 'Hess 2009 Toy Race Car and Racer', 2009, 'Car has working headlights, taillights, and running lights along with horn, ignition, and engine acceleration sounds', 11.99,  '2009_1.jpg'), " +
                " ('95ca027a-a18d-4007-8d92-ed92e862cea8', 'Hess 2011 Toy Truck and Racer', 2011, 'Truck has 34 functioning lights along with ignition, back up alert, hydraulic ramp and a horn sound.', 11.99, '2011_1.jpg'), " +
                " ('028b1e54-6008-47c1-a7ef-6ba5ae5540a1', 'Hess 2012 Helicopter and Rescue SUV', 2012, 'The chopper has working lights along with searchlights. Sounds include, landing and take off', 11.99,  '2012_1.jpg'), " +
                " ('a4dec62a-ae82-4e4f-965a-b5779e799ac2', 'Hess 2013 Truck & Tractor', 2013, 'The truck has  front and rear lights. Sounds include truck horn, ignition sound, beeping back up warning sound. Tractor has front and rear lights', 11.99,  '2013_1.jpg'), " +
                " ('cb8433fc-fff0-48c8-a2f9-b92cb66582d5', 'Hess 2014 Toy Truck & Space Cruiser With Scout', 2014, 'Truck has front, rear and running lights. Sounds include the truck starting, a horn, and the warning sound as the ramp comes out.', 11.99,  '2014_1.jpg'); ";

            await dbContext.Database.ExecuteSqlRawAsync(truckSql);
            //dbContext.Trucks.FromSqlRaw(truckSql);
        }

        private static async Task PopulateCategories(CatalogDbContext dbContext)
        {
            string categoriesSql = "insert into categories(name,is_mini_truck, category_order) values " +
               " ('1980s Hess Trucks',false, 1980), " +
               " ('1990s Hess Trucks',false, 1990), " +
               " ('2000s Hess Trucks',false, 2000), " +
               " ('2010s Hess Trucks',false, 2010) ";
            await dbContext.Database.ExecuteSqlRawAsync(categoriesSql);
        }

        private static async Task PopulateTruckCategories(CatalogDbContext dbContext)
        {
            int[] decades = new int[4] { 1980, 1990, 2000, 2010 };
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < decades.Length; i++)
            {
                var truckCategorySql = await TruckCategorySqlByDecade(dbContext, decades[i]);
                if (truckCategorySql != string.Empty)
                    sb.Append(truckCategorySql);
            }
            if (sb.Length > 0)
            {
                sb.Insert(0, "insert into category_truck(categories_category_id, trucks_truck_id) values ");
                sb.Replace(",", ";", sb.Length - 1, 1);

                await dbContext.Database.ExecuteSqlRawAsync(sb.ToString());
            }
        }

        static async Task<string> TruckCategorySqlByDecade(CatalogDbContext dbContext, int decadeStart)
        {
            var decadeTrucks = await dbContext.Trucks.Where(t => t.Year >= decadeStart && t.Year < decadeStart + 10).ToListAsync();
            var decadeCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryOrder == decadeStart);
            StringBuilder sb = new StringBuilder();
            foreach (var truck in decadeTrucks)
            {
                var truckCategorySql = $" ({decadeCategory.CategoryId}, '{truck.TruckId}'),";
                sb.Append(truckCategorySql);
            }
            return sb.ToString();
        }

        private static async Task PopulatePhotos(CatalogDbContext dbContext)
        {

            StringBuilder sb = new StringBuilder();
            foreach (var truck in dbContext.Trucks)
            {
                sb.Append($"('/images{truck.Year}_1.jpg','{truck.TruckId}'),('/images{truck.Year}_2.jpg','{truck.TruckId}'),");
            }
            if (sb.Length > 0)
            {
                sb.Insert(0, "insert into photos(photo_path, truck_id) values ");
                sb.Replace(",", ";", sb.Length - 1, 1);

                await dbContext.Database.ExecuteSqlRawAsync(sb.ToString());
            }
        }
    }
}
