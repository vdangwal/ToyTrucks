using EventBus.Messages.Events;
using Inventory.Api.DbContexts;
using Inventory.Api.Models;
using MassTransit;
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
        private readonly IPublishEndpoint _publishEndpoint;


        public InventoryRepository(InventoryDbContext context, ILogger<InventoryRepository> logger, IPublishEndpoint publishEndpoint)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
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
                                                .SingleOrDefaultAsync(t => t.TruckName == truckInventory.TruckName);
            if (truckInventoryToUpdate == null)
            {
                _logger.LogError("Truck inventory to update doesnt exist in the db");
                throw new ArgumentNullException(nameof(truckInventoryToUpdate));
            }
            if (truckInventoryToUpdate.Quantity - truckInventory.Quantity < 0)
            {
                throw new ArgumentException($"{truckInventory.TruckName} does not contain {truckInventory.Quantity} trucks");
            }
            else
            {
                truckInventoryToUpdate.Quantity -= truckInventory.Quantity;

                var result = await SaveChanges();
                if (truckInventoryToUpdate.Quantity <= 0)
                {
                    var eventMessage = new InventorySoldOutEvent();// _mapper.Map<BasketCheckoutEvent>(basketCheckout);
                    eventMessage.TruckId = truckInventory.TruckId;
                    await _publishEndpoint.Publish(eventMessage);
                }
                return result;
            }
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