using System;
using System.ComponentModel.DataAnnotations;

namespace Basket.Api.Dtos
{
    public class BasketForCreation
    {
        [Required]
        public Guid UserId { get; set; }
    }
}