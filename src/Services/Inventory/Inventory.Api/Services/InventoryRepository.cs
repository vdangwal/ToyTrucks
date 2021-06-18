using Inventory.Api.Models;
using System.Threading.Tasks;

namespace Inventory.Api.Services
{
    public class InventoryRepository : IInventoryRepository
    {
        public async Task<TruckInventoryDto> GetTruckInventory(string truckName)
        {
            return null;
        }
        //Task AddTruckInventory(TruckInventoryDto truckInventory);
        public void UpdateTruckInventory(TruckInventoryDto truckInventory)
        {

        }
        public async Task<bool> SaveChanges()
        {
            return false;
        }
    }
}