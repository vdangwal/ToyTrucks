using System;

namespace Web.Models.View
{
    public class BasketLineViewModel
    {
        public string LineId { get; set; }
        public Guid TruckId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Year { get; set; }
        public string DefaultPhotoPath { get; set; }
    }
}
