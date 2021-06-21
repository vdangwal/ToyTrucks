using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Events;
using Inventory.Api.Entities;

namespace Inventory.Api.Profiles
{
    public class TruckInventoryProfile : Profile
    {
        public TruckInventoryProfile()
        {
            CreateMap<TruckInventory, Models.TruckInventoryDto>()
                    .ReverseMap();

            CreateMap<UpdatedInventory, Models.TruckInventoryDto>()
                                    .ForMember(d => d.Id, s => s.Ignore())//.MapFrom(i => i.ProductId))
                                    .ForMember(d => d.TruckName, s => s.MapFrom(i => i.ProductName))
                                    ;

        }
    }
}
