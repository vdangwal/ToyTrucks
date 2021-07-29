using System;

namespace Basket.Api.Dtos
{
    public class Truck
    {
        public Guid TruckId { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string DefaultPhotoPath { get; set; }
    }
}