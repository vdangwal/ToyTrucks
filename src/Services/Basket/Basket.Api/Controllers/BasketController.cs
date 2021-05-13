using System;
using System.Threading.Tasks;
using AutoMapper;
using Basket.Api.Dtos;
using Basket.Api.GrpcServices;
using Basket.Api.Models;
using Basket.Api.Services;
using Discount.Grpc.Protos;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{

    [ApiVersion("1.0")]
    [Route("api/basket")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _service;
        private readonly IMapper _mapper;
        private readonly DiscountGrpcService _discountService;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(IBasketRepository service, IMapper mapper, DiscountGrpcService discountService, IPublishEndpoint publishEndpoint)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _discountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        [HttpGet]
        public string GetTime()
        {
            return DateTime.Now.ToString();
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));

            var basketDto = await _service.GetBasket(userName);
            var basket = _mapper.Map<ShoppingCart>(basketDto);

            return Ok((basket != null) ? basket : new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            if (basket == null)
                throw new ArgumentNullException(nameof(basket));

            foreach (var item in basket.Items)
            {

                var discount = await _discountService.GetDiscount(item.ProductName);
                //   if (discount is not null)
                if (discount.Coupon is not null)
                {
                    item.Price -= discount.Coupon.Amount;
                    Console.WriteLine($"discount for {item.ProductName}. New price is {item.Price}");
                }
                else
                    Console.WriteLine($"No discount for {item.ProductName}");

            }

            return Ok(await _service.UpdateBasket(_mapper.Map<Dtos.ShoppingCartDto>(basket)));
        }


        [HttpDelete("{userName}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));
            await _service.DeleteBasket(userName);
            return NoContent();
        }

        [HttpOptions]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetBasketOptions()
        {
            base.Response.Headers.Add("Allow", "GET, POST, OPTIONS, PATCH, DELETE");
            return Ok();
        }
        [Route("[action]")] //we need to add method name to url ie
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            if (basketCheckout == null)
                throw new ArgumentNullException(nameof(basketCheckout));
            //get existing basket with username
            //crete basketcheckoutevent
            //set total price on basketcheckout
            //SEND CEHCKOUTevent to rabbit
            //empty the basket

            var basket = await _service.GetBasket(basketCheckout.UserName);
            if (basket == null)
                return BadRequest();

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;

            await _publishEndpoint.Publish(eventMessage);

            await _service.DeleteBasket(basketCheckout.UserName);
            return Accepted();


        }
    }
}