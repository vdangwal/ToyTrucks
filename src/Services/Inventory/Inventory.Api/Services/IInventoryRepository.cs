using System;
using System.Collections.Generic;
using Inventory.Api.Models;
using System.Threading.Tasks;

namespace Inventory.Api.Services
{
    public interface IInventoryRepository
    {
        //  Task<IEnumerable<TruckInventoryDto>> GetCategories();
        //         Task<IEnumerable<Category>> GetCategoriesBySize(bool isMini = false);
        Task<TruckInventoryDto> GetTruckInventory(Guid truckId);
        Task<IEnumerable<TruckInventoryDto>> GetAllTruckInventory();
        //Task AddTruckInventory(TruckInventoryDto truckInventory);
        Task<bool> UpdateTruckInventory(TruckInventoryDto truckInventory);
        Task<int> HasEnoughQuantity(Guid truckId, int quantityWanted);
        Task<bool> SaveChanges();
    }
}