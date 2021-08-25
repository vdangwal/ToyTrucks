using System;

namespace EventBus.Messages.Events
{
    public class Inventory
    {
        public Guid TruckId { get; set; }
        public string TruckName { get; set; }
        public int Quantity { get; set; }
    }
}