using System.Linq;
using System.Collections.Generic;

namespace Orders.Api.Models
{
    public class OrderBasket
    {
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();


        public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);
    }
}