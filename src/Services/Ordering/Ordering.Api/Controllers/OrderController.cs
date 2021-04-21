using Microsoft.AspNetCore.Mvc;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using System.Net;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;

namespace Ordering.Api.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public OrderController(IMediator mediatr)
        {
            _mediatr = mediatr ?? throw new ArgumentNullException(nameof(mediatr));
        }

        [HttpGet("{userName:string}", Name = "OrdersByUsername")]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> OrdersByUsername(string userName)
        {
            var query = new GetOrdersListQuery(userName);
            var orders = await _mediatr.Send(query);
            return Ok(orders);
        }

        [HttpPost(Name = "CheckoutOrder")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand order)
        {
            var id = await _mediatr.Send(order);
            return Ok(id);
        }

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<int>> UpdateOrder([FromBody] UpdateOrderCommand order)
        {
            await _mediatr.Send(order);
            return NoContent();
        }

        [HttpDelete("{orderId:int}", Name = "DeleteOrder")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<int>> DeleteOrder(int orderId)
        {
            var command = new DeleteOrderCommand() { Id = orderId };
            await _mediatr.Send(command);
            return NoContent();
        }
    }
}