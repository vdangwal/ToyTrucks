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

            CreateMap<EventBus.Messages.Events.Inventory, Models.TruckInventoryDto>()
                               .ReverseMap();
            CreateMap<InventoryToUpdate, Models.TruckInventoryDto>()
                                .ForMember(d => d.TruckName, s => s.MapFrom(i => i.TruckName))
                                ;

        }
    }
}
