using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
            System.Console.WriteLine($"IN ORDERS COnsumer with {context.Message.UserId}");
            var basketCheckoutDetails = (BasketCheckoutEvent)context.Message;
            var order = new Order
            {
                UserId = basketCheckoutDetails.UserId,
                OrderPaid = false,
                OrderPlaced = DateTime.Now,
                TotalPrice = basketCheckoutDetails.BasketTotal
            };
            order.OrderId = Guid.NewGuid();
            order.OrderItems = new List<OrderItem>();
            foreach (var bLine in basketCheckoutDetails.Basket)
            {
                OrderItem item = new OrderItem
                {
                    OrderLineId = Guid.NewGuid(),
                    OrderId = order.OrderId,
                    Price = bLine.Price,
                    Quantity = bLine.Quantity,
                    TruckId = bLine.TruckId,
                    TruckName = bLine.Name,

                };
                order.OrderItems.Add(item);
            }
            var returnOrder = await _service.AddOrderAsync(order);
            await UpdateInventory(context.Message.Basket);
            Console.WriteLine("Order added");
            _logger.LogInformation("Order added");
        }

        private async Task UpdateInventory(List<BasketItemEvent> cart)
        {
            if (cart == null || cart.Any() == false)
                return;
            foreach (var item in cart)
            {

                var eventMessage = new InventoryToUpdate();
                eventMessage.TruckId = item.TruckId;
                eventMessage.TruckName = item.Name;
                eventMessage.Quantity = item.Quantity;
                Console.WriteLine($"trying to update inventory for {eventMessage.TruckName } TruckId = {eventMessage.TruckId} ProductId = {item.Id}");
                await _publishEndpoint.Publish(eventMessage);
                _logger.LogInformation($"Update Inventory event published for Name {eventMessage.TruckName} with new quantity {eventMessage.Quantity}");
            }

        }
    }

}