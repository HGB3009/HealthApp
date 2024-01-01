using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareApp.Models
{
    public class WaterPerDay
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; }
        [BsonElement("Day")]
        public string Day { get; set; }
        [BsonElement("AmountOfWater")]
        public double AmountOfWater { get; set; }
    }
}
