using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Core
{
	public class User
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		public string Username { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }
		public DateTime CreatedAt { get; set; }
		
		public string Name { get; set; }          
		public int Age { get; set; }              
		public string Bio { get; set; }           
		public string Location { get; set; }      
		public string ProfilePicture { get; set; } 
	}
}
	

