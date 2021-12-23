﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToyTrucks.Catalog.Api.Models
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsMiniTruck { get; set; }
        public int CategoryOrder { get; set; }
        //  public virtual ICollection<TruckDto> Trucks { get; set; }
    }
}
