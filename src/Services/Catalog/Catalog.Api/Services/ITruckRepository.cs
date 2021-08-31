using Catalog.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Services
{
    public interface ITruckRepository
    {
        Task<IEnumerable<Truck>> GetTrucks();
        Task<IEnumerable<Truck>> GetTrucksByCategoryId(int categoryId);
        Task<Truck> GetTruckById(Guid truckId);
        Task<Truck> GetTruckByName(string truckName);
        Task AddTruck(Truck truck);
        Task<bool> UpdateTruck(Truck truck);
        Task<bool> UpdateTruckInventory(TruckInventory truckInventory);
        Task<bool> SaveChanges();
    }
}
