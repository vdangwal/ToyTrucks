using AutoMapper;
using Catalog.Api.Models;
using Catalog.Api.Entities;
using EventBus.Messages.Events;
namespace Catalog.Api.Profiles
{
    public class AllProfiles : Profile
    {
        public AllProfiles()
        {
            CreateMap<TruckInventory, TruckInventoryDto>()
                    .ReverseMap();

            CreateMap<InventoryToUpdate, TruckInventory>()
                     .ReverseMap();
        }
    }
}