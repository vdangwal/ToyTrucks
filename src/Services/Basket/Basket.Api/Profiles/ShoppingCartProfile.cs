using AutoMapper;

namespace Basket.Api.Profiles
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            CreateMap<Models.ShoppingCart, Dtos.ShoppingCartDto>().ReverseMap();
            CreateMap<Models.ShoppingCartItem, Dtos.ShoppingCartItemDto>().ReverseMap();
        }
    }
}