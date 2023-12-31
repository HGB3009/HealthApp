using System;

namespace HealthCareApp.Models
{
    public class ReminderModel
    {
        // Drink Reminder properties
        public double Volume { get; set; }
        public double ConsumedWater { get; set; }
        public double TargetWater { get; set; }

        // Meal Reminder properties
        public DateTime MealTime { get; set; }
        public string Description { get; set; }

        // Doctor Appointment properties
        public DateTime AppointmentDateTime { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public string AppointmentLocation { get; set; }

        // Common properties
        public bool IsEnabled { get; set; }
        public DateTime ReminderTime { get; set; }

        // Combined methods
        public void UpdateConsumedWater(double amount)
        {
            ConsumedWater += amount;
        }

        public bool IsTargetAchieved() 
        {
            return ConsumedWater >= TargetWater;
        }
    }
}