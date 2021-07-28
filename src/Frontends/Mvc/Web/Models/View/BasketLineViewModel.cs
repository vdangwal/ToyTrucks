using System;

namespace Web.Models.View
{
    public class BasketLineViewModel
    {
        public Guid LineId { get; set; }
        public Guid TruckId { get; set; }
        public string TruckName { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
