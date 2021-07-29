using AutoMapper;
using Basket.Api.Dtos;

namespace Basket.Api.Profiles
{
    public class AllProfiles : Profile
    {
        public AllProfiles()
        {
            base.CreateMap<BasketForCreation, Dtos.Basket>()
                .ReverseMap();
        }
    }
}