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
    [ApiVersion("2.0")]
    //[Route("api/v{version:apiVersion}/discount")]
    [Route("api/discount")]
    public class DiscountControllerV2 : ControllerBase
    {
        private readonly IDiscountRepository _repository;
        private readonly IMapper _mapper;

        public DiscountControllerV2(IDiscountRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Coupon>> GetDiscounts()
        {
            var discounts = new List<Coupon>{
                new Coupon{ProductName="THIS IS ONLY FOR TESTING!", Amount=4,Description="fwef"},
                new Coupon{ProductName="THIS IS ONLY FOR TESTING!", Amount=4,Description="fwefwwewe"},
            };


            return Ok(discounts);
        }


    }
}