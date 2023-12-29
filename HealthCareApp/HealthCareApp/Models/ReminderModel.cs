using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCareApp.Models;

namespace HealthCareApp.Models
{
    public class ReminderModel
    {
        public class DrinkReminder
        {
            public double ConsumedWater { get; set; }
            public double TargetWater { get; set; }
            public bool IsEnabled { get; set; }
            public DateTime ReminderTime { get; set; }

        }
        public class MealReminder 
        {
            public DateTime MealTime { get; set; }
            public required string Description { get; set; }
            public bool IsEnabled { get; set; }
            public DateTime ReminderTime { get; set; }
        }
        public class DoctorAppointment
        {
            public DateTime AppointmentDateTime { get; set; }
            public string DoctorName { get; set; }
            public string Specialization { get; set; }
            public string AppointmentLocation { get; set; }
            public bool IsEnabled { get; set; }
            public DateTime ReminderTime { get; set; }
        }
    }
}