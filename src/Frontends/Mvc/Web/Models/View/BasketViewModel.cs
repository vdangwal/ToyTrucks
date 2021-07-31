using System;
using System.Collections.Generic;

namespace Web.Models.View
{
    public class BasketViewModel
    {
        public Guid BasketId { get; set; }
        public List<BasketLineViewModel> BasketLines { get; set; }
        public decimal ShoppingCartTotal { get; set; }
    }
}
