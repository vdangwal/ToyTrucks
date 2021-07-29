using System;
using System.Threading.Tasks;
using AutoMapper;
using dtos = Basket.Api.Dtos;
//using EventBus.Messages.Events;
using Basket.Api.GrpcServices;

using Basket.Api.Services;
//using Discount.Grpc.Protos;
//using Basket.Api.Events;
using MassTransit;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Basket.Api.Dtos;
using System.Collections.Generic;

namespace Basket.Api.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/baskets/v2/{basketId}/basketlines")]
    [ApiController]
    public class BasketLinesController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IBasketLinesService _basketLinesService;

        private readonly IMapper _mapper;
        private readonly DiscountGrpcService _discountService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BasketController> _logger;

        public BasketLinesController(IBasketService service, IMapper mapper, DiscountGrpcService discountService, IPublishEndpoint publishEndpoint, ILogger<BasketController> logger, IBasketLinesService basketLinesService)
        {
            _basketService = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _discountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _basketLinesService = basketLinesService ?? throw new ArgumentNullException(nameof(basketLinesService));
        }



        [HttpGet(Name = "GetBasketLines")]
        public async Task<ActionResult<IEnumerable<BasketLine>>> GetBasketLines(Guid basketId)
        {
            if (basketId == Guid.Empty)
            {
                return NotFound();
            }
            var basketLines = await _basketLinesService.GetBasketLinesById(basketId);
            if (basketLines != null)
            {
                return Ok(basketLines);
            }
            return NotFound();
        }


    }
}