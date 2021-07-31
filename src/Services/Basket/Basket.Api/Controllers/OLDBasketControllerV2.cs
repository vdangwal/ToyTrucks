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
    [Route("api/baskets/v2")]
    [ApiController]
    public class BasketControllerV2 : ControllerBase
    {
        // private readonly IBasketService _basketService;
        // private readonly IMapper _mapper;
        // private readonly DiscountGrpcService _discountService;
        // private readonly IPublishEndpoint _publishEndpoint;
        // private readonly ILogger<BasketControllerV2> _logger;

        // public BasketControllerV2(IBasketService service, IMapper mapper, DiscountGrpcService discountService, IPublishEndpoint publishEndpoint, ILogger<BasketControllerV2> logger)
        // {
        //     _basketService = service ?? throw new ArgumentNullException(nameof(service));
        //     _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        //     _discountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
        //     _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        //     _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        // }

        // [HttpGet("{basketId:Guid}", Name = "GetBasket")]
        // public async Task<ActionResult<dtos.Basket>> GetBasket(Guid basketId)
        // {
        //     if (basketId == Guid.Empty)
        //     {
        //         return NotFound();
        //     }
        //     var basket = await _basketService.GetBasketById(basketId);
        //     if (basket != null)
        //     {
        //         return Ok(basket);
        //     }
        //     return NotFound();
        // }

        // // [HttpGet("{basketId:Guid}", Name = "GetBasket")]
        // // public async Task<ActionResult<IEnumerable<BasketLine>>> GetBasketLines(Guid basketId)
        // // {
        // //     if (basketId == Guid.Empty)
        // //     {
        // //         return NotFound();
        // //     }
        // //     var basketLines = await _basketService.GetBasketLinesById(basketId);
        // //     if (basketLines != null)
        // //     {
        // //         return Ok(babasketLinessket);
        // //     }
        // //     return NotFound();
        // // }

        // [HttpPost(Name = "AddBasket")]
        // public ActionResult<dtos.Basket> AddBasket(BasketForCreation basketToCreate)
        // {
        //     if (basketToCreate == null)
        //     {
        //         return NotFound();
        //     }
        //     var basket = _mapper.Map<dtos.Basket>(basketToCreate);

        //     _basketService.AddBasket(basket);


        //     var basketToReturn = _mapper.Map<dtos.Basket>(basket);
        //     return CreatedAtRoute(
        //        "GetBasket",
        //        new { basketId = basket.BasketId },
        //        basketToReturn);
        // }
    }
}