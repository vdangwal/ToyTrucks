using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using EventBus.Messages.Events;
using Inventory.Api.Services;
using Inventory.Api.Models;
using AutoMapper;

namespace Inventory.Api.Events
{
    public class InventoryToUpdateConsumer : IConsumer<InventoryToUpdate>
    {
        private readonly ILogger<InventoryToUpdateConsumer> _logger;
        private readonly IInventoryRepository _service;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        public InventoryToUpdateConsumer(ILogger<InventoryToUpdateConsumer> logger, IInventoryRepository service, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        public async Task Consume(ConsumeContext<InventoryToUpdate> context)
        {
            if (context == null)
            {
                _logger.LogWarning("Failed to consume Inventory update as context is null");
                return;
            }
            TruckInventoryDto tid = _mapper.Map<TruckInventoryDto>(context.Message);
            if (await _service.UpdateTruckInventory(tid) == true)
            {
                TruckInventoryDto newDetails = await _service.GetTruckInventory(tid.TruckId);

                //inform baskets of updated inventory
                var eventMessage = new UpdatedInventory();
                eventMessage.TruckId = context.Message.TruckId;
                eventMessage.TruckName = newDetails.TruckName;
                eventMessage.NewQuantity = newDetails.Quantity;
                Console.WriteLine($"trying to update inventory for {eventMessage.TruckName }");
                await _publishEndpoint.Publish(eventMessage);

            }

            // return Task.CompletedTask;
        }
    }
}