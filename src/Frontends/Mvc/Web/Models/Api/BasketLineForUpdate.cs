using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Api
{
    public class BasketLineForUpdate
    {
        [Required]
        public Guid LineId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
