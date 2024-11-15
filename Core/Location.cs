using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Core
{
    public class Location
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault] 
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Id { get; set; }  // Gør Id nullable

        [BsonElement("classroom")]
        public string Classroom { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }
    }
}