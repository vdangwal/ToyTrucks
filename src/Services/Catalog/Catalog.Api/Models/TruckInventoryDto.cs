using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Catalog.Api.Models
{
    public class TruckInventoryDto
    {
        public Guid TruckId { get; set; }
        public string TruckName { get; set; }
        public int Quantity { get; set; }
    }
}