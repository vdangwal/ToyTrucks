using System;
using System.ComponentModel.DataAnnotations;
namespace Inventory.Api.Models
{
    public class TruckInventoryDto
    {
        [Key]
        public Guid TruckId { get; set; }
        public string TruckName { get; set; }
        public int Quantity { get; set; }
    }
}