using AutoMapper;

namespace Basket.Api.Profiles
{
    public class BasketCheckoutProfile : Profile
    {
        public BasketCheckoutProfile()
        {
            CreateMap<Models.BasketCheckout, EventBus.Messages.Events.BasketCheckoutEvent>().ReverseMap();
        }
    }
}