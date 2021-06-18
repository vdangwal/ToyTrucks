using Inventory.Api.DbContexts;
using Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Inventory.Api.Services
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly InventoryDbContext _context;
        private readonly ILogger<InventoryRepository> _logger;

        public InventoryRepository(InventoryDbContext context, ILogger<InventoryRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }


        public async Task<TruckInventoryDto> GetTruckInventory(string truckName)
        {
            if (string.IsNullOrEmpty(truckName))
            {
                _logger.LogError("truckName to query is null");
                throw new ArgumentException(nameof(truckName));
            }
            return await _context.TruckInventory
                                        .FirstOrDefaultAsync(t => t.TruckName == truckName);
        }
        //Task AddTruckInventory(TruckInventoryDto truckInventory);
        public async Task<bool> UpdateTruckInventory(TruckInventoryDto truckInventory)
        {
            if (truckInventory == null)
                throw new ArgumentNullException(nameof(truckInventory));

            var truckInventoryToUpdate = await _context.TruckInventory
                                                .SingleOrDefaultAsync(t => t.Id == truckInventory.Id);
            if (truckInventoryToUpdate == null)
            {
                _logger.LogError("Truck inventory to update doesnt exist in the db");
                throw new ArgumentNullException(nameof(truckInventoryToUpdate));
            }
            truckInventoryToUpdate.Quantity = truckInventory.Quantity;
            return await SaveChanges();
        }

        public async Task<int> HasEnoughQuantity(string truckName, int quantityWanted)
        {
            if (string.IsNullOrEmpty(truckName))
            {
                _logger.LogError("truckName to query is null");
                throw new ArgumentException(nameof(truckName));
            }

            var truckInventory = await _context.TruckInventory
                                                .SingleOrDefaultAsync(t => t.TruckName == truckName);
            if (truckInventory == null)
            {
                _logger.LogError("Truck inventory to check quantity doesnt exist in the db");
                throw new ArgumentNullException(nameof(truckInventory));
            }

            return truckInventory.Quantity - quantityWanted;

        }
        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}