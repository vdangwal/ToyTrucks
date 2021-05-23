using System;
using System.Collections.Generic;
using System.Linq;

namespace Basket.Api.Dtos
{
    public class ShoppingCartDto
    {
        public ShoppingCartDto() { }
        public ShoppingCartDto(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; set; }

        public List<ShoppingCartItemDto> Items { get; set; } = new List<ShoppingCartItemDto>();


        public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);
    }
}