using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
//using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

using Orders.Api.Services;
using Orders.Api.Models;
using EventBus.Messages.Events;
namespace Orders.Api.Entities
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IOrdersRepository _service;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketCheckoutConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketCheckoutConsumer(IMapper mapper, ILogger<BasketCheckoutConsumer> logger, IOrdersRepository service, IPublishEndpoint publishEndpoint)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            System.Console.WriteLine($"IN ORDERS COnsumer with {context.Message.UserName}");
            var order = _mapper.Map<OrderDto>(context.Message);
            // foreach (var item in context.Message.Basket.Items)
            // {
            //     order.OrderItems.Add(_mapper.Map<OrderItem>(item));
            // }
            // //  order.OrderItems = _mapper.Map<List<OrderItem>>(context.Message.Basket);
            var returnOrder = await _service.AddOrderAsync(order);
            await UpdateInventory(context.Message.Basket);
            Console.WriteLine("Order added");
            _logger.LogInformation("Order added");
        }

        private async Task UpdateInventory(ShoppingCart cart)
        {
            if (cart == null || cart.Items.Any() == false)
                return;
            foreach (var item in cart.Items)
            {

                var eventMessage = new InventoryToUpdate();
                eventMessage.TruckId = item.ProductId;
                eventMessage.ProductName = item.ProductName;
                eventMessage.Quantity = item.Quantity;
                Console.WriteLine($"trying to update inventory for {eventMessage.ProductName } TruckId = {eventMessage.TruckId} ProductId = {item.ProductId}");
                await _publishEndpoint.Publish(eventMessage);
                _logger.LogInformation($"Update Inventory event published for Name {eventMessage.ProductName} with new quantity {eventMessage.Quantity}");
            }

        }
    }

}