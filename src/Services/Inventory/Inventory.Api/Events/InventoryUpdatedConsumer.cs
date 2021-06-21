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
    public class InventoryUpdatedConsumer : IConsumer<UpdatedInventory>
    {
        private readonly ILogger<InventoryUpdatedConsumer> _logger;
        private readonly IInventoryRepository _service;
        private readonly IMapper _mapper;
        public InventoryUpdatedConsumer(ILogger<InventoryUpdatedConsumer> logger, IInventoryRepository service, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task Consume(ConsumeContext<UpdatedInventory> context)
        {
            if (context == null)
            {
                _logger.LogWarning("Failed to consume Inventory update as context is null");
                return;
            }
            TruckInventoryDto tid = _mapper.Map<TruckInventoryDto>(context.Message);
            await _service.UpdateTruckInventory(tid);

            // return Task.CompletedTask;
        }
    }
}