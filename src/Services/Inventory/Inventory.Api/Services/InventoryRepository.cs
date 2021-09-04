using EventBus.Messages.Events;
using Inventory.Api.DbContexts;
using Inventory.Api.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public async Task<TruckInventoryDto> GetTruckInventory(Guid truckId)
        {
            return await _context.TruckInventory
                               .FirstOrDefaultAsync(t => t.TruckId == truckId);
        }

        public async Task<IEnumerable<TruckInventoryDto>> GetAllTruckInventory()
        {
            return await _context.TruckInventory.ToListAsync();
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

        public async Task<int> HasEnoughQuantity(Guid truckId, int quantityWanted)
        {
            if (truckId == Guid.Empty)
            {
                _logger.LogError("truckId to query is null");
                throw new ArgumentException(nameof(truckId));
            }

            var truckInventory = await _context.TruckInventory
                                                .SingleOrDefaultAsync(t => t.TruckId == truckId);
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