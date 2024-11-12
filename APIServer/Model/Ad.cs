using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace APIServer.Model
{
	public class Ad
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string UserId { get; set; } // Reference til User

		public string Title { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
		public string Status { get; set; }
		public string ImageUrl { get; set; }
		public DateTime CreatedAt { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string CategoryId { get; set; } // Reference til Category

		[BsonRepresentation(BsonType.ObjectId)]
		public string LocationId { get; set; } // Reference til Location
	}
}
