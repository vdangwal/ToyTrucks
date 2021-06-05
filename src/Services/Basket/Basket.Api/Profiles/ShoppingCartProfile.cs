using AutoMapper;

namespace Basket.Api.Profiles
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            CreateMap<Models.ShoppingCart, Dtos.ShoppingCartDto>().ReverseMap();
            CreateMap<Models.ShoppingCartItem, Dtos.ShoppingCartItemDto>().ReverseMap();

            //  CreateMap<Models.ShoppingCart, EventBus.Messages.Events.ShoppingCart>().ReverseMap();
            CreateMap<Dtos.ShoppingCartDto, EventBus.Messages.Events.ShoppingCart>().ReverseMap();
            CreateMap<Dtos.ShoppingCartItemDto, EventBus.Messages.Events.ShoppingCartItem>().ReverseMap();
            //   CreateMap<Models.ShoppingCartItem, EventBus.Messages.Events.ShoppingCartItem>().ReverseMap();
        }
    }
}