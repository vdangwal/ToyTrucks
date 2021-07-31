using AutoMapper;
using EventBus.Messages.Events;
namespace Basket.Api.Profiles
{
    public class OLDBasketCheckoutProfile : Profile
    {
        public OLDBasketCheckoutProfile()
        {
           // CreateMap<Basket.Api.Dtos.BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}