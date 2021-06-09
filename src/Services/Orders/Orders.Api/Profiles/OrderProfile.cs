using AutoMapper;
// using EventBus.Messages.Events;
using MongoDB.Bson;
using Orders.Api.Entities;
using Orders.Api.Models;
using Orders.Api.Events;
namespace Orders.Api.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                    //  .ForMember(d => d.Id, s => s.MapFrom(a => new ObjectId(a.Id)))
                    .ReverseMap();
            CreateMap<OrderDto, BasketCheckoutEvent>()
                    .ForMember(d => d.Basket, s => s.MapFrom(a => a.OrderItems))
                    .ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        }
    }
}