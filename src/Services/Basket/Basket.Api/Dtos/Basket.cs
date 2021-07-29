using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Basket.Api.Dtos
{
    public class Basket
    {
        [BsonId]//(IdGenerator = typeof(CombGuidGenerator))]
        public Guid BasketId { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public Collection<BasketLine> BasketLines { get; set; }
        public Guid? Coupon { get; set; }
    }
}