using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Inventory.Api.Models
{
    public class TruckInventoryDto
    {
        public int Id { get; set; }
        [NotMapped]
        public Guid TruckId { get; set; }
        public string TruckName { get; set; }
        public int Quantity { get; set; }
    }
}