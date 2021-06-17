using AutoMapper;
using EventBus.Messages.Events;
namespace Basket.Api.Profiles
{
    public class BasketCheckoutProfile : Profile
    {
        public BasketCheckoutProfile()
        {
            CreateMap<Basket.Api.Dtos.BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}