using System.Collections.Generic;
using Web.Models.Api;

namespace Web.Models.View
{
    public class TruckListModel
    {
        public IEnumerable<Truck> Trucks { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public int NumberOfItems { get; set; }
        public int? SelectedCategory { get; set; }
    }
}