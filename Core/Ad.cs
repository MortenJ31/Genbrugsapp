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
		public string? UserId { get; set; } // Reference til User
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
        public string? CategoryId { get; set; } // Reference til Category

        [BsonElement("locationId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? LocationId { get; set; } // Reference til Location
	}
}
