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
                    .ReverseMap();
            CreateMap<Order, OrderDto>()
                    .ReverseMap();
            CreateMap<Order, BasketCheckoutEvent>()
                    .ForMember(d => d.Basket, s => s.MapFrom(a => a.OrderItems))
                    .ReverseMap();
        }
    }
}