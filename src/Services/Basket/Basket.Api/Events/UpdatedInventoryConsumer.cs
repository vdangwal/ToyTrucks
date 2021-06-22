using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using EventBus.Messages.Events;
using Basket.Api.Services;
using AutoMapper;

namespace Basket.Api.Events
{
    public class UpdatedInventoryConsumer : IConsumer<UpdatedInventory>
    {

        private readonly ILogger<UpdatedInventoryConsumer> _logger;
        private readonly IBasketRepository _service;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        public UpdatedInventoryConsumer(ILogger<UpdatedInventoryConsumer> logger, IBasketRepository service, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        public async Task Consume(ConsumeContext<UpdatedInventory> context)
        {
            if (context == null)
            {
                _logger.LogWarning("Failed to consume updated inventory as context is null");
                return;
            }
            var updatedInventory = (UpdatedInventory)context.Message;
            System.Console.WriteLine($"In Basket updated consumer !!!!! TruckId = {updatedInventory.TruckId}");
        }
    }
}