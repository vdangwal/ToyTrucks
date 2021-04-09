using AutoMapper;
using Discount.API.Models;
using Discount.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Discount.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    //[Route("api/v{version:apiVersion}/discount")]
    [Route("api/discount")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _repository;
        private readonly IMapper _mapper;

        public DiscountController(IDiscountRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coupon>>> GetDiscounts()
        {
            var discounts = await _repository.GetDiscounts();
            return Ok(_mapper.Map<IEnumerable<API.Dtos.Coupon>, IEnumerable<API.Models.Coupon>>(discounts));
        }

        [HttpGet("{productName}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            var discount = await _repository.GetDiscount(productName);
            return Ok(_mapper.Map<API.Models.Coupon>(discount));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
        {
            var couponDto = _mapper.Map<API.Dtos.Coupon>(coupon);
            var resultCouponDto = await _repository.CreateDiscount(couponDto);
            return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, _mapper.Map<API.Models.Coupon>(resultCouponDto));
        }

        [HttpPut]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Coupon>> UpdateDiscount([FromBody] Coupon coupon)
        {
            var couponDto = _mapper.Map<API.Dtos.Coupon>(coupon);
            var isUpdated = await _repository.UpdateDiscount(couponDto);
            return isUpdated ? NoContent() : BadRequest();
        }

        [HttpDelete("{productName}", Name = "DeleteDiscount")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<bool>> DeleteDiscount(string productName)
        {
            var isDeleted = await _repository.DeleteDiscount(productName);
            return isDeleted ? NoContent() : BadRequest();
        }
    }
}