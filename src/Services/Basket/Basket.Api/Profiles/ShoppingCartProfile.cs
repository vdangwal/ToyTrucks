using AutoMapper;
using EventBus.Messages.Events;

namespace Basket.Api.Profiles
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            CreateMap<ShoppingCart, Dtos.ShoppingCartDto>().ReverseMap();
            CreateMap<ShoppingCartItem, Dtos.ShoppingCartItemDto>().ReverseMap();
        }
    }
}