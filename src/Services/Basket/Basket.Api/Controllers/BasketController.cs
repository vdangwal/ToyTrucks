using System;
using System.Threading.Tasks;
using AutoMapper;
using Basket.Api.Dtos;
using EventBus.Messages.Events;
using Basket.Api.GrpcServices;
using Basket.Api.Models;
using Basket.Api.Services;
using Discount.Grpc.Protos;
using Basket.Api.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<BasketController> _logger;

        public BasketController(IBasketRepository service, IMapper mapper, DiscountGrpcService discountService, IPublishEndpoint publishEndpoint, ILogger<BasketController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _discountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

                var discount = await _discountService.GetDiscount(item.ProductId);
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

            var eventMessage = new tempevent() { UserName = "tizzer" };


            await _publishEndpoint.Publish(eventMessage);


            // var basket = await _service.GetBasket(basketCheckout.UserName);
            // if (basket == null)
            //     return BadRequest();

            // var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            // eventMessage.Basket = _mapper.Map<ShoppingCart>(basket);
            // eventMessage.TotalPrice = basket.TotalPrice;

            // await _publishEndpoint.Publish(eventMessage);

            await _service.DeleteBasket(basketCheckout.UserName);
            return Accepted();


        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateDiscount([FromBody] Coupon coupon)
        {
            if (coupon == null)
                throw new ArgumentNullException(nameof(coupon));

            var discount = await _discountService.CreateDiscount(coupon);
            if (discount == null)
                return BadRequest();

            return Accepted();
        }

        [HttpGet("[action]/{productId}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productId)
        {
            if (string.IsNullOrEmpty(productId))
                return BadRequest();

            var discountResponse = await _discountService.GetDiscount(productId);
            if (discountResponse?.Coupon == null)
                return NotFound();
            // var basket = _mapper.Map<ShoppingCart>(basketDto);

            return Ok(discountResponse.Coupon);
        }

        [HttpDelete("[action]/{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteDiscount(string productId)
        {
            if (string.IsNullOrEmpty(productId))
                return BadRequest();

            var discountResponse = await _discountService.DeleteDiscount(productId);
            if (discountResponse == null)
                return NotFound();
            // var basket = _mapper.Map<ShoppingCart>(basketDto);

            return NoContent();
        }

        [HttpPut(Name = "UpdateDiscount")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateDiscount([FromBody] Coupon coupon)
        {
            if (coupon == null)
                return BadRequest();


            try
            {
                await _discountService.UpdateDiscount(coupon);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error updating discount with name of {coupon.ProductName}");
                return NotFound();
            }

            //await _mediatr.Send(order);
            return NoContent();
        }

        [Route("[action]")] //we need to add method name to url ie
        [HttpGet]
        public async Task<ActionResult> Border([FromBody] SampleData eve)
        {


            //var eventMessage = new tempevent() { UserName = "tizzer" };


            await _publishEndpoint.Publish(eve);


            return Accepted();


        }

        [Route("[action]")] //we need to add method name to url ie
        [HttpGet]
        public async Task<ActionResult> orderfuck([FromBody] SampleData eve)
        {


            //var eventMessage = new tempevent() { UserName = "tizzer" };


            await _publishEndpoint.Publish(eve);


            return Accepted();


        }
    }
}