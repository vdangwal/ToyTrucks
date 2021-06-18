using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Inventory.Api.Entities;

namespace Inventory.Api.Profiles
{
    public class TruckInventoryProfile : Profile
    {
        public TruckInventoryProfile()
        {
            CreateMap<TruckInventory, Models.TruckInventoryDto>()
                    .ReverseMap();
        }
    }
}
