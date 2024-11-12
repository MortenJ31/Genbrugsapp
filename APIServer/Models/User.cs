﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace APIServer.Model
{
	public class User
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		public string Username { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}