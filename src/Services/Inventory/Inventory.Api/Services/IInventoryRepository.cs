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
        Task<bool> UpdateTruckInventory(TruckInventoryDto truckInventory);
        Task<int> HasEnoughQuantity(string truckName, int quantityWanted);
        Task<bool> SaveChanges();
    }
}