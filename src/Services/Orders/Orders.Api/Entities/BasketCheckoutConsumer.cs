using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using AutoMapper;
//using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Orders.Api.Events;
using Orders.Api.Services;
using Orders.Api.Models;

namespace Orders.Api.Entities
{
    // public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    // {
    //     private readonly IOrdersRepository _service;
    //     private readonly IMapper _mapper;
    //     private readonly ILogger<BasketCheckoutConsumer> _logger;
    //     public BasketCheckoutConsumer(IMapper mapper, ILogger<BasketCheckoutConsumer> logger, IOrdersRepository service)
    //     {
    //         _mapper = mapper;
    //         _logger = logger;
    //         _service = service;
    //     }

    //     public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    //     {

    //         var order = _mapper.Map<OrderDto>(context.Message);
    //         // foreach (var item in context.Message.Basket.Items)
    //         // {
    //         //     order.OrderItems.Add(_mapper.Map<OrderItem>(item));
    //         // }
    //         // //  order.OrderItems = _mapper.Map<List<OrderItem>>(context.Message.Basket);
    //         var returnOrder = await _service.AddOrderAsync(order);
    //         Console.WriteLine("Order added");
    //         _logger.LogInformation("Order added");
    //     }
    // }
    public class BasketCheckoutConsumer : IConsumer<tempevent>
    {



        public Task Consume(ConsumeContext<tempevent> context)
        {

            // var order = _mapper.Map<OrderDto>(context.Message);
            // foreach (var item in context.Message.Basket.Items)
            // {
            //     order.OrderItems.Add(_mapper.Map<OrderItem>(item));
            // }
            // //  order.OrderItems = _mapper.Map<List<OrderItem>>(context.Message.Basket);
            // var returnOrder = await _service.AddOrderAsync(order);
            Console.WriteLine(context.Message.UserName);
            // _logger.LogInformation("Order added");
            return Task.CompletedTask;
        }
    }
}