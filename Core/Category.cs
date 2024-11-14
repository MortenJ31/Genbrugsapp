using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  

        [BsonElement("name")]  
        public string Name { get; set; }  

        [BsonElement("description")]  
        public string Description { get; set; }  
    }
}