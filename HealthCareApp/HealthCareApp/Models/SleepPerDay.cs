using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareApp.Models
{
    public class SleepPerDay
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("Day")]
        public string Day{ get; set; }
        [BsonElement("Hour")]
        public double Hour { get; set; }
    }
    public static class SleepExtensions
    {
        public static List<SleepPerDay> ConvertToSleepPerDayList(this Sleep sleep)
        {
            List<SleepPerDay> sleepPerDayList = new List<SleepPerDay>();

            DateTime startDate = DateTime.ParseExact(sleep.StartDay, "dd/MM/yyyy", null);
            DateTime endDate = DateTime.ParseExact(sleep.EndDay, "dd/MM/yyyy", null);

            while (startDate.Date <= endDate.Date)
            {
                DateTime currentDayStart = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
                DateTime currentDayEnd = new DateTime(startDate.Year, startDate.Month, startDate.Day, 23, 59, 59);

                DateTime endtime = DateTime.ParseExact($"{sleep.EndDay} {sleep.EndTime}", "dd/MM/yyyy HH:mm", null);
                DateTime starttime = DateTime.ParseExact($"{sleep.StartDay} {sleep.StartTime}", "dd/MM/yyyy HH:mm", null);

                if (endtime < starttime)
                {
                    endtime = endtime.AddDays(1);
                }

                DateTime currentDayStartInSleep = starttime > currentDayStart ? starttime : currentDayStart;
                DateTime currentDayEndInSleep = endtime < currentDayEnd ? endtime : currentDayEnd;

                SleepPerDay sleepPerDay = new SleepPerDay
                {
                    Username = sleep.Username,
                    Day = currentDayStartInSleep.ToString("dd/MM/yyyy"),
                    Hour = CalculateHourForDay(currentDayStartInSleep, currentDayEndInSleep)
                };

                sleepPerDayList.Add(sleepPerDay);

                // Check if there is a portion of sleep on the next day
                if (endtime.Date > currentDayEnd.Date)
                {
                    DateTime nextDayStartTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0).AddDays(1);

                    DateTime nextDayStartInSleep = starttime > nextDayStartTime ? starttime : nextDayStartTime;
                    DateTime nextDayEndInSleep = endtime < currentDayEnd ? endtime : currentDayEnd;

                    SleepPerDay nextDaySleep = new SleepPerDay
                    {
                        Username = sleep.Username,
                        Day = nextDayStartInSleep.ToString("dd/MM/yyyy"),
                        Hour = CalculateHourForDay(nextDayStartInSleep, nextDayEndInSleep)
                    };

                    sleepPerDayList.Add(nextDaySleep);
                }

                startDate = startDate.AddDays(1);
            }

            return sleepPerDayList;
        }

        private static double CalculateHourForDay(DateTime currentDayStart, DateTime currentDayEnd)
        {
            TimeSpan sleepDuration = currentDayEnd - currentDayStart;
            return (double)sleepDuration.TotalHours;
        }
    }


}
