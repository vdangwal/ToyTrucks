using System;
using System.Threading.Tasks;
using AutoMapper;
using Basket.Api.GrpcServices;
using Basket.Api.Models;
using Basket.Api.Services;
using Discount.Grpc.Protos;
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

        public BasketController(IBasketRepository service, IMapper mapper, DiscountGrpcService discountService)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _discountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
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

            return Ok(await _service.UpdateBasket(_mapper.Map<Dtos.ShoppingCart>(basket)));
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
    }
}