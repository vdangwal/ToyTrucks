using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Orders.Api.Services;

namespace Orders.Api.Entities
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IOrdersRepository _service;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketCheckoutConsumer> _logger;
        public BasketCheckoutConsumer(IMapper mapper, ILogger<BasketCheckoutConsumer> logger, IOrdersRepository service)
        {
            _mapper = mapper;
            _logger = logger;
            _service = service;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var order = _mapper.Map<Order>(context.Message);
            var returnOrder = await _service.AddOrderAsync(order);
            _logger.LogInformation("Order added");
        }
    }
}