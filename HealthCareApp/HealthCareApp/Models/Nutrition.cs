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
    public class Nutrition
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("MealName")]
        public string MealName { get; set; }

        [BsonElement("MealTime")]
        public string MealTime { get; set; }
        [BsonElement("Day")]
        public string Day { get; set; }

        [BsonElement("Quantity")]
        public int Quantity { get; set; }

        [BsonElement("Unit")]
        public string Unit { get; set; }

        [BsonElement("Calories")]
        public double Calories { get; set; }

        [BsonElement("MealPicture")]
        public byte[] MealPicture { get; set; }
        public BitmapImage MealPictureSource
        {
            get
            {
                if (MealPicture != null && MealPicture.Length > 0)
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = new System.IO.MemoryStream(MealPicture);
                    image.EndInit();
                    return image;
                }
                return null;
            }
        }
    }
}
