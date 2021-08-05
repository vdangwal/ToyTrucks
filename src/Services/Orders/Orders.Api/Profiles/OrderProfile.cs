using AutoMapper;
using EventBus.Messages.Events;
using MongoDB.Bson;
using Orders.Api.Entities;
using Orders.Api.Models;

namespace Orders.Api.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {

            CreateMap<OrderLineDto, OrderItem>()
                    //  .ForMember(d => d.Id, s => s.MapFrom(a => new ObjectId(a.Id)))
                    .ReverseMap();
            CreateMap<Order, OrderDto>()
            //  .ForMember(d => d.Id, s => s.MapFrom(a => new ObjectId(a.Id)))
                    .ReverseMap();
            CreateMap<Order, BasketCheckoutEvent>()
                    .ForMember(d => d.Basket, s => s.MapFrom(a => a.OrderItems))
                    .ReverseMap();

            // CreateMap<OrderBasket, ShoppingCart>().ReverseMap();
            // CreateMap<OrderItemDto, ShoppingCartItem>().ReverseMap();

            // CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        }
    }
}