using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Catalog.Api.Filters;
using Catalog.Api.Models;
using Catalog.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace Catalog.Api.Controllers
{
    [Route("api/trucks")]
    [ApiController]
    public class TruckController : ControllerBase
    {
        private readonly ITruckRepository _service;
        private IMapper _mapper;
        private readonly ILogger<TruckController> _logger;
        public TruckController(ITruckRepository service, IMapper mapper, ILogger<TruckController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        [Route("{categoryId:int}")]
        [TrucksFilter]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<TruckDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<TruckDto>>> TrucksByCategory(int categoryId)
        {
            var trucks = await _service.GetTrucksByCategoryId(categoryId);
            if (trucks == null)
                return NotFound();
            return Ok(trucks);
        }

        [HttpGet]
        [Route("{truckId:Guid}")]
        [TruckFilter]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(TruckDto), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<TruckDto>> TruckById(Guid truckId)
        {
            if (truckId == Guid.Empty)
                return BadRequest();

            var truck = await _service.GetTruckById(truckId);
            if (truck == null)
                return NotFound();
            return Ok(truck);
        }

        [HttpPut]
        [TruckFilter]
        public async Task<ActionResult<TruckDto>> UpdateTruck([FromBody] TruckDto truckDto)
        {
            if (truckDto == null)
                return BadRequest();
            var truck = _mapper.Map<Entities.Truck>(truckDto);
            //_service.UpdateTruck(truck);
            if (await _service.UpdateTruck(truck))
                return Ok();
            else
                return BadRequest();
        }
    }
}
