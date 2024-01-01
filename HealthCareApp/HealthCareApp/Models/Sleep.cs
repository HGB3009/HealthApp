using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.DirectoryServices.ActiveDirectory;

namespace HealthCareApp.Models
{
    class Sleep
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("StartTime")]
        public string StartTime { get; set; }

        [BsonElement("StartDay")]
        public string StartDay { get; set; }

        [BsonElement("EndTime")]
        public string EndTime { get; set; }
        [BsonElement("EndDay")]
        public string EndDay { get; set; }

        [BsonElement("Type")]
        public string Type { get; set; }

        [BsonElement("Quality")]
        public string Quality { get; set; }

        public string SleepTime
        {
            get
            {
                
                if (StartDay != EndDay)
                {
                    string SleepTime;

                    DateTime endtime = DateTime.ParseExact(EndTime, "HH:mm", null);
                    DateTime starttime = DateTime.ParseExact(StartTime, "HH:mm", null);

                    DateTime midnight1 = new DateTime(24, 0, 0);
                    DateTime midnight2 = new DateTime(0, 0, 0);

                    TimeSpan duration1 = midnight1 - starttime;
                    TimeSpan duration2 = endtime - midnight2;

                    TimeSpan totalDuration = duration1 + duration2;
                    int hours = totalDuration.Hours;
                    int minutes = totalDuration.Minutes;

                    return ($"Sleep Time: {hours} hours {minutes} minutes");
                }
                else
                {
                    string SleepTime;
                    DateTime endtime = DateTime.ParseExact(EndTime, "HH:mm", null);
                    DateTime starttime = DateTime.ParseExact(StartTime, "HH:mm", null);
                    TimeSpan sleepDuration = endtime - starttime;
                    int hours = sleepDuration.Hours;
                    int minutes = sleepDuration.Minutes;

                    return($"Sleep Time: {hours} hours {minutes} minutes");
                }
            }
        }
    }
}
