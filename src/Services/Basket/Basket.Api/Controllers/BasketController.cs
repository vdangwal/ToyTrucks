using AutoMapper;
using Basket.Api.Dtos;
using Basket.Api.Events;
using Basket.Api.Services;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventBus.Messages.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
namespace Basket.Api.Controllers
{
    [Route("api/v1/basket")]
    //[Authorize]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        //private readonly IIdentityService _identityService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BasketController> _logger;
        private readonly IMapper _mapper;

        public BasketController(
            ILogger<BasketController> logger,
            IBasketRepository repository,
            //IIdentityService identityService,
            IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            //   _identityService = identityService;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> GetBasketByIdAsync(string id)
        {
            var basket = await _repository.GetBasketAsync(id);

            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody] CustomerBasket value)
        {
            return Ok(await _repository.UpdateBasketAsync(value));
        }

        [Route("checkout")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // var userId = _identityService.GetUserIdentity();

            // basketCheckout.RequestId = (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty) ?
            //     guid : basketCheckout.RequestId;

            var basket = await _repository.GetBasketAsync(basketCheckout.BasketId);

            if (basket == null)
            {
                return BadRequest();
            }

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.Basket = new List<BasketItemEvent>();
            decimal total = 0.0M;
            foreach (var item in basket.Items)
            {
                var basketItemEvent = _mapper.Map<BasketItemEvent>(item);
                total += item.Price * item.Quantity;
                eventMessage.Basket.Add(basketItemEvent);
            }

            Coupon coupon = null;

            //     if (basket.CouponId.HasValue)
            //         coupon = await discountService.GetCoupon(basket.CouponId.Value);

            //     //if (basket.CouponId.HasValue)
            //     //    coupon = await discountService.GetCouponWithError(basket.CouponId.Value);

            if (coupon != null)
            {
                eventMessage.BasketTotal = total - coupon.Amount;
            }
            else
            {
                eventMessage.BasketTotal = total;
            }


            // Once basket is checkout, sends an integration event to
            // ordering.api to convert basket to order and proceeds with
            // order creation process
            try
            {
                await _publishEndpoint.Publish(eventMessage);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
            await _repository.DeleteBasketAsync(basketCheckout.BasketId);
            return Accepted(eventMessage);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task DeleteBasketByIdAsync(string id)
        {
            await _repository.DeleteBasketAsync(id);
        }
    }
}