using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Core
{
    public class Purchase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; }

        [BsonElement("adId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? AdId { get; set; }

        [BsonElement("purchaseDate")]
        public DateTime PurchaseDate { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        [BsonElement("locationId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? LocationId { get; set; }
        
     
      
        public bool IsSelected { get; set; }
        
        [BsonElement("itemName")]
        public string ItemName { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

    }
}
