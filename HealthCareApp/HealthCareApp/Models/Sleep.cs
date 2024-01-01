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
    public class Sleep
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
                    string endtimestr = $"{EndDay} {EndTime}";
                    string starttimestr = $"{StartDay} {StartTime}";

                    DateTime endtime = DateTime.ParseExact(endtimestr, "dd/MM/yyyy HH:mm", null);
                    DateTime starttime = DateTime.ParseExact(starttimestr, "dd/MM/yyyy HH:mm", null);

                    TimeSpan sleepDuration = endtime - starttime;
                    double hours = Math.Abs((int)sleepDuration.TotalHours);
                    int minutes = Math.Abs(sleepDuration.Minutes);

                    return ($"{hours} hours {minutes} minutes");
                }
                else
                {
                    DateTime endtime = DateTime.ParseExact(EndTime, "HH:mm", null);
                    DateTime starttime = DateTime.ParseExact(StartTime, "HH:mm", null);
                    TimeSpan sleepDuration = endtime - starttime;
                    int hours = Math.Abs(sleepDuration.Hours);
                    int minutes = Math.Abs(sleepDuration.Minutes);

                    return($"{hours} hours {minutes} minutes");
                }
            }
        }
    }
}
