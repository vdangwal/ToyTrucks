using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models.Api;
namespace Web.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<Truck>> GetTrucksByCategoryId(int categoryId);
        Task<IEnumerable<Truck>> GetTrucks();
        Task<Truck> GetTruckById(Guid truckId);
        Task<IEnumerable<Category>> GetCategories();

    }
}