using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Basket.Api.Dtos
{
    public class BasketLine
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid BasketLineId { get; set; }

        [Required]
        public Guid BasketId { get; set; }

        [Required]
        public Guid EventId { get; set; }
        public Truck Truck { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int Price { get; set; }

        public Basket Basket { get; set; }
    }
}