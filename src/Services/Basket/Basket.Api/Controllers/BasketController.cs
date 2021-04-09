using System;
using System.Threading.Tasks;
using AutoMapper;
using Basket.Api.Models;
using Basket.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    [Route("api/basket")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _service;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper;
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