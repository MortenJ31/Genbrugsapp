using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace APIServer.Model
{
	public class Purchase
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string UserId { get; set; } // Reference til User

		[BsonRepresentation(BsonType.ObjectId)]
		public string AdId { get; set; } // Reference til Ad

		public DateTime PurchaseDate { get; set; }
		public string Status { get; set; }
	}
}
