using Microsoft.AspNetCore.Mvc;
//using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using System.Net;
using AutoMapper;
using Orders.Api.Services;
using Orders.Api.Models;
using Entities = Orders.Api.Entities;
using Microsoft.Extensions.Logging;

namespace Orders.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        // private readonly IMediator _mediatr;
        private readonly IOrdersRepository _service;
        private IMapper _mapper;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrdersRepository service, IMapper mapper, ILogger<OrderController> logger)
        {

            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("{orderId::length(24)}")]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<OrderDto>> OrderById(string orderId)
        {
            try
            {
                var orders = await _service.GetByOrderIdAsync(orderId);
                return Ok(orders);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error getting order with id of {orderId} as it does not exist");
                return NotFound();
            }

        }

        [HttpGet("{userName:alpha}", Name = "OrdersByUsername")]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> OrdersByUsername(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return BadRequest();
            var orders = await _service.GetOrdersByUserName(userName);
            return Ok(orders);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> AllOrders()
        {
            // var query = new GetOrdersListQuery(userName);
            //var orders = await _mediatr.Send(query);
            var orders = await _service.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpPost(Name = "CheckoutOrder")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] Entities.Order order)
        {
            if (order == null)
                return BadRequest();
            var orderDto = _mapper.Map<OrderDto>(order);
            var returnOrder = await _service.AddOrderAsync(orderDto);
            return Ok(returnOrder?.Id);
        }

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<int>> UpdateOrder([FromBody] Entities.Order order)
        {
            if (order == null)
                return BadRequest();

            var orderDto = _mapper.Map<OrderDto>(order);
            try
            {
                await _service.UpdateOrderAsync(orderDto);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error updating order with UserName of {order.UserName} as it does not exist");
                return NotFound();
            }

            //await _mediatr.Send(order);
            return NoContent();
        }

        [HttpDelete("{orderId:alpha}", Name = "DeleteOrder")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<int>> DeleteOrder(string orderId)
        {
            try
            {
                await _service.DeleteOrderAsync(orderId);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error deleting order with id of {orderId} as it does not exist");
                return NotFound();
            }
            return NoContent();
        }
    }
}