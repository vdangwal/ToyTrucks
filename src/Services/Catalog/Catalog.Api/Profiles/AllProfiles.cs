using AutoMapper;
using Catalog.Api.Models;
using Catalog.Api.Entities;
namespace Catalog.Api.Profiles
{
    public class AllProfiles : Profile
    {
        public AllProfiles()
        {
            CreateMap<TruckInventory, TruckInventoryDto>()
                    .ReverseMap();
        }
    }
}