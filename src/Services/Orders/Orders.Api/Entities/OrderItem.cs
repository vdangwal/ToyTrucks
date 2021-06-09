using System;

namespace Orders.Api.Entities
{
    public class OrderItem
    {

        //private string _pictureUrl;
        // private decimal _discount;
        public int Quantity { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }

    }
}