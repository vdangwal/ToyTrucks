using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using EventBus.Messages.Events;

using AutoMapper;

namespace Catalog.Api.Events
{
    public class InventoryReturnedConsumer : IConsumer<InventoryToReturn>
    {
        private readonly ILogger<InventoryReturnedConsumer> _logger;
        //private readonly IInventoryRepository _service;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        public InventoryReturnedConsumer(ILogger<InventoryReturnedConsumer> logger, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            //     _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        public async Task Consume(ConsumeContext<InventoryToReturn> context)
        {
            var basketCheckoutDetails = (InventoryToReturn)context.Message;

            // return Task.CompletedTask;
        }
    }
}