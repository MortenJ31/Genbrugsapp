using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class PurchaseWithAdDetails
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string AdId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Status { get; set; }
        public string LocationId { get; set; }

        // Ad details - ingen BsonElement nødvendig her
        public string AdTitle { get; set; }
        public string AdDescription { get; set; }
        public double AdPrice { get; set; }
        public string AdImageUrl { get; set; }
    }
}
