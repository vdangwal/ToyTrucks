using AutoMapper;
using EventBus.Messages.Events;
using Orders.Api.Entities;
using Orders.Api.Models;

namespace Orders.Api.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderDto, Order>().ReverseMap();
            CreateMap<Order, BasketCheckoutEvent>().ReverseMap();
        }
    }
}