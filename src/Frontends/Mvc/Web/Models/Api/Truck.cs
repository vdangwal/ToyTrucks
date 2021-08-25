using System;
using System.Collections.Generic;

namespace Web.Models.Api
{
    public class Truck
    {
        public Guid TruckId { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? PreviousPrice { get; set; }
        public bool Hidden { get; set; }
        public bool Damaged { get; set; }
        public string DefaultPhotoPath { get; set; }
        public bool OutOfStock { get; set; }
        public int Quantity { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}