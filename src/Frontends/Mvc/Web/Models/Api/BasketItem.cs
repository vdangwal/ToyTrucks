using System;
namespace Web.Models.Api
{
    public class BasketItem
    {
        public string Id { get; set; }
        public Guid TruckId { get; set; }
        public string TruckName { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }
    }
}