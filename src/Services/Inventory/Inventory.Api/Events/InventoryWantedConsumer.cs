using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using EventBus.Messages.Events;
using Inventory.Api.Services;
using Inventory.Api.Models;
using AutoMapper;

namespace Inventory.Api.Events
{
    public class InventoryWantedConsumer : IConsumer<InventoryWanted>
    {
        private readonly ILogger<InventoryWantedConsumer> _logger;
        private readonly IInventoryRepository _service;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        public InventoryWantedConsumer(ILogger<InventoryWantedConsumer> logger, IInventoryRepository service, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        public async Task Consume(ConsumeContext<InventoryWanted> context)
        {

            var truckInventory = await _service.GetAllTruckInventory();

            var truckInventoryForCatalog = _mapper.Map<IEnumerable<EventBus.Messages.Events.Inventory>>(truckInventory);

            var eventMessage = new InventoryToReturn();
            eventMessage.TruckInventory = truckInventoryForCatalog;
            await _publishEndpoint.Publish(eventMessage);
            // return Task.CompletedTask;
        }
    }
}