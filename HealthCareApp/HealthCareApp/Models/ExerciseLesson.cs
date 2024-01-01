using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareApp.Models
{
    public class ExerciseLesson
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("ExerciseName")]
        public string ExerciseName { get; set; }

        [BsonElement("ExerciseType")]
        public string ExerciseType{ get; set; }
        [BsonElement("DayStart")]
        public DateTime? ExerciseDayStart { get; set; }

        [BsonElement("DayEnd")]
        public DateTime? ExerciseDayEnd { get; set; }

        [BsonElement("ExerciseTime")]
        public string ExerciseTime { get; set; }

        [BsonElement("Equipment")]
        public string Equipment { get; set; }
        [BsonElement("Calories")]
        public string Calories { get; set; }
    }
}
