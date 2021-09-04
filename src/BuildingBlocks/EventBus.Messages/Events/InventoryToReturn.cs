using System;
using System.Collections.Generic;
namespace EventBus.Messages.Events
{
    public class InventoryToReturn
    {
        public IEnumerable<Inventory> TruckInventory { get; set; }
    }
}