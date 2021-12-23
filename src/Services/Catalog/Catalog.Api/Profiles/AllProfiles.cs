using AutoMapper;
using ToyTrucks.Catalog.Api.Models;
using ToyTrucks.Catalog.Api.Entities;
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