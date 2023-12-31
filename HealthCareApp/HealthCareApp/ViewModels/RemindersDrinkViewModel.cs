using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace HealthCareApp.ViewModels
{
    public class RemindersDrinkViewModel : INotifyPropertyChanged
    {
        private int _targetWaterAmount;
        private ObservableCollection<ReminderTime> _reminderTimes;
        private ICommand _saveCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public RemindersDrinkViewModel()
        {
            ReminderTimes = new ObservableCollection<ReminderTime>
            {
                new ReminderTime { Time = "06:30", IsAlarmSet = true },
                new ReminderTime { Time = "08:30", IsAlarmSet = true },
                new ReminderTime { Time = "10:30", IsAlarmSet = true },
                new ReminderTime { Time = "12:30", IsAlarmSet = true },
                new ReminderTime { Time = "14:30", IsAlarmSet = true },
                new ReminderTime { Time = "16:30", IsAlarmSet = true },
                new ReminderTime { Time = "18:30", IsAlarmSet = true },
                new ReminderTime { Time = "20:30", IsAlarmSet = true }
            };

            _saveCommand = new RelayCommand(SaveData);
        }

        public int TargetWaterAmount
        {
            get => _targetWaterAmount;
            set
            {
                if (_targetWaterAmount != value)
                {
                    _targetWaterAmount = value;
                    OnPropertyChanged(nameof(TargetWaterAmount));
                    UpdateWaterAmounts();
                }
            }
        }

        public ObservableCollection<ReminderTime> ReminderTimes
        {
            get => _reminderTimes;
            set
            {
                _reminderTimes = value;
                OnPropertyChanged(nameof(ReminderTimes));
            }
        }

        public ICommand SaveCommand => _saveCommand;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateWaterAmounts()
        {
            if (_reminderTimes != null)
            {
                int amountPerReminder = TargetWaterAmount / _reminderTimes.Count;
                foreach (var reminder in _reminderTimes)
                {
                    reminder.WaterAmount = amountPerReminder;
                }
            }
        }

        private void SaveData(object obj)
        {
            
        }
    }

    public class ReminderTime : INotifyPropertyChanged
    {
        private string _time;
        private int _waterAmount;
        private bool _isAlarmSet;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Time
        {
            get => _time;
            set
            {
                if (_time != value)
                {
                    _time = value;
                    OnPropertyChanged(nameof(Time));
                }
            }
        }

        public int WaterAmount
        {
            get => _waterAmount;
            set
            {
                if (_waterAmount != value)
                {
                    _waterAmount = value;
                    OnPropertyChanged(nameof(WaterAmount));
                }
            }
        }

        public bool IsAlarmSet
        {
            get => _isAlarmSet;
            set
            {
                if (_isAlarmSet != value)
                {
                    _isAlarmSet = value;
                    OnPropertyChanged(nameof(IsAlarmSet));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}