using System;

namespace EventBus.Messages.Events
{
    public class InventorySoldOutEvent
    {
        public Guid TruckId { get; set; }

    }
}