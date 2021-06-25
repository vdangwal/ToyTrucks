using System;

namespace EventBus.Messages.Events
{
    public class ShoppingCartItem
    {
        public int Quantity { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public bool OutOfStock { get; set; }
    }
}