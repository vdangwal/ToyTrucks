using AutoMapper;
using Basket.Api.Events;
using Basket.Api.Dtos;

namespace Basket.Api.Profiles
{
    public class AllProfiles : Profile
    {
        public AllProfiles()
        {
            CreateMap<BasketCheckoutEvent, BasketCheckout>()
                .ReverseMap();
            CreateMap<BasketItemEvent, BasketItem>()
           .ReverseMap();
        }
    }
}