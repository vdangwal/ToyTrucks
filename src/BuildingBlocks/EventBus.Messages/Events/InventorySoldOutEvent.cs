using System;

namespace ToyTrucks.EventBus.Messages.Events
{
    public class InventorySoldOutEvent
    {
        public Guid TruckId { get; set; }

    }
}