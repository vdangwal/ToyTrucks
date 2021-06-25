using System;
namespace Inventory.Api.Entities
{
    public class TruckInventory
    {
        public int Id { get; set; }

        public Guid TruckId { get; set; }
        public string TruckName { get; set; }
        public int Quantity { get; set; }
    }
}