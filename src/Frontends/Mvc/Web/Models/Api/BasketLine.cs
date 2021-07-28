using System;

namespace Web.Models.Api
{
    public class BasketLine
    {
        public Guid BasketLineId { get; set; }
        public Guid BasketId { get; set; }
        public Guid TruckId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public Truck Truck { get; set; }
    }
}
