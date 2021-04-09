using AutoMapper;

namespace Basket.Api.Profiles
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            CreateMap<Models.ShoppingCart, Dtos.ShoppingCart>().ReverseMap();
            CreateMap<Models.ShoppingCartItem, Dtos.ShoppingCartItem>().ReverseMap();
        }
    }
}