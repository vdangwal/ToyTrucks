using System;
namespace Basket.Api.Dtos
{
    public class ShoppingCartItemDto
    {
        public int Quantity { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public bool OutOfStock { get; set; }

    }
}