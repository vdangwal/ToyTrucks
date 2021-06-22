using System;
namespace EventBus.Messages.Events
{
    public class InventoryToUpdate
    {
        public int Quantity { get; set; }

        public Guid TruckId { get; set; }
        public string ProductName { get; set; }
    }
}