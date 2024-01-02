using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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
        [BsonElement("Weight ")]
        public double Weight { get; set; }
        [BsonElement("Height")]
        public double Height { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("Avatar")]
        public byte[] Avatar { get; set; }

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
        public BitmapImage AvatarImageSource
        {
            get
            {
                if (Avatar != null && Avatar.Length > 0)
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = new System.IO.MemoryStream(Avatar);
                    image.EndInit();
                    return image;
                }
                return null;
            }
        }
    }
}
