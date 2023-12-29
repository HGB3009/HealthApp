using HealthCareApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareApp.ViewModels
{
    public class RemindersDrinkViewModel : INotifyPropertyChanged
    {
        private ReminderModel.DrinkReminder _drinkReminder;

        public double ConsumedWater
        {
            get { return _drinkReminder.ConsumedWater; }
            set
            {
                _drinkReminder.ConsumedWater = value;
                OnPropertyChanged(nameof(ConsumedWater));
            }
        }

        public double TargetWater
        {
            get { return _drinkReminder.TargetWater; }
            set
            {
                _drinkReminder.TargetWater = value;
                OnPropertyChanged(nameof(TargetWater));
            }
        }

        public bool IsEnabled
        {
            get { return _drinkReminder.IsEnabled; }
            set
            {
                _drinkReminder.IsEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        public DateTime ReminderTime
        {
            get { return _drinkReminder.ReminderTime; }
            set
            {
                _drinkReminder.ReminderTime = value;
                OnPropertyChanged(nameof(ReminderTime));
            }
        }

        public RemindersDrinkViewModel(ReminderModel.DrinkReminder drinkReminder)
        {
            _drinkReminder = drinkReminder;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}