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
    public class InventoryUpdatedConsumer : IConsumer<InventoryToUpdate>
    {
        private readonly ILogger<InventoryUpdatedConsumer> _logger;
        private readonly IInventoryRepository _service;
        private readonly IMapper _mapper;
        // private readonly IPublishEndpoint _publishEndpoint;
        public InventoryUpdatedConsumer(ILogger<InventoryUpdatedConsumer> logger, IInventoryRepository service, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            //    _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
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
                TruckInventoryDto newDetails = await _service.GetTruckInventory(tid.TruckName);


                // var eventMessage = new InventoryToUpdate();
                // //   eventMessage.ProductId = item.ProductId;
                // eventMessage.ProductName = item.ProductName;
                // eventMessage.Quantity = item.Quantity;
                // Console.WriteLine($"trying to update inventory for {eventMessage.ProductName }");
                // await _publishEndpoint.Publish(eventMessage);

            }

            // return Task.CompletedTask;
        }
    }
}