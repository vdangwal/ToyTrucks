using System;
namespace Catalog.Api.Entities
{
    public class TruckInventory
    {
        public Guid TruckId { get; set; }
        public int Quantity { get; set; }
    }
}