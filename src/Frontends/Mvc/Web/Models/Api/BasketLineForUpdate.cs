using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Api
{
    public class BasketLineForUpdate
    {
        [Required]
        public string LineId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
