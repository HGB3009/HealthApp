using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareApp.Models
{
    internal class UserInformation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Gender")]
        public string Gender { get; set; }

        [BsonElement("Phone number")]
        public string PhoneNumber { get; set; }

        [BsonElement("Birthday")]
        public string Birthday { get; set; }

        [BsonElement("Address")]
        public string Address { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; }
        
        public UserInformation()
        {
            Id = "";
            Name = "";
            Gender = "";
            PhoneNumber = "";
            Birthday = "";
            Address = "";
            Email = "";
            Username = "";
        }    
    }
}
