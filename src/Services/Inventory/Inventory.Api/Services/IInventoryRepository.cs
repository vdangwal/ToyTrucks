using Inventory.Api.Models;
using System.Threading.Tasks;

namespace Inventory.Api.Services
{
    public interface IInventoryRepository
    {
        //  Task<IEnumerable<TruckInventoryDto>> GetCategories();
        //         Task<IEnumerable<Category>> GetCategoriesBySize(bool isMini = false);
        Task<TruckInventoryDto> GetTruckInventory(string truckName);
        //Task AddTruckInventory(TruckInventoryDto truckInventory);
        void UpdateTruckInventory(TruckInventoryDto truckInventory);
        Task<bool> SaveChanges();
    }
}