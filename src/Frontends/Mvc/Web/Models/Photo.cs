using System;
namespace NewWeb.Models
{
    public class Photo
    {
        public Guid PhotoId { get; set; }
        public string PhotoPath { get; set; }

        public Guid TruckId { get; set; }
        public Truck Truck { get; set; }
    }
}