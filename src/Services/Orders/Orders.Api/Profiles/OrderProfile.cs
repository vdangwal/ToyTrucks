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
            CreateMap<OrderDto, Order>()
                    .ForMember(d => d.Id, s => s.MapFrom(a => new ObjectId(a.Id)))
                    .ReverseMap();
            CreateMap<Order, BasketCheckoutEvent>().ReverseMap();
            CreateMap<OrderItem, ShoppingCartItem>().ReverseMap();
        }
    }
}