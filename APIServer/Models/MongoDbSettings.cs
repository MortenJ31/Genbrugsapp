using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace APIServer.Models
{
	public class MongoDbSettings
	{
		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }
	}
}
