using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models;
namespace NewWeb.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<Truck>> GetTrucksByCategoryId(int categoryId);
        Task<Truck> GetTruckById(Guid truckId);

    }
}