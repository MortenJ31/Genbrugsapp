using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Core
{
    public class Ad
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; } // Ændret til string for konsistens

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("categoryId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; } // Holdes som string

        [BsonElement("locationId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string LocationId { get; set; } // Holdes som string

        [BsonElement("locationName")]
        public string LocationName { get; set; }

        [BsonElement("locationAddress")]
        public string LocationAddress { get; set; }
    }
}