using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Api
{
    public class BasketLineForCreation
    {
        [Required]
        public Guid TruckId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Price { get; set; }
    }
}
