using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using EventBus.Messages.Events;
namespace Inventory.Api.Events
{
    public class InventoryUpdatedConsumer : IConsumer<UpdatedInventory>
    {
        private readonly ILogger<InventoryUpdatedConsumer> _logger;

        public InventoryUpdatedConsumer(ILogger<InventoryUpdatedConsumer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Consume(ConsumeContext<UpdatedInventory> context)
        {
            _logger.LogInformation("In Inventory updated!!!!!!!!!!!!!!!!!!");
            return Task.CompletedTask;
        }
    }
}