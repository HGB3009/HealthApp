    using HealthCareApp.Models;
    using HealthCareApp.Views;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    namespace HealthCareApp.ViewModels
    {
        public class SleepTrackerViewModel : BaseViewModel
        {
            private readonly IMongoCollection<Sleep> _sleepCollection;

            private string _username;
            public string UserNameVM
            {
                get { return _username; }
                set
                {
                    if (_username != value)
                    {
                        _username = value;
                        OnPropertyChanged(nameof(UserNameVM));
                    }
                }
            }
            private DateTime? _starttime;
            public DateTime? StartTimeVM
            {
                get { return _starttime; }
                set
                {
                    if (_starttime != value)
                    {
                        _starttime = value;
                        OnPropertyChanged(nameof(StartTimeVM));
                    }
                }
            }
            private DateTime? _startday;
            public DateTime? StartDayVM
            {
                get { return _startday; }
                set
                {
                    if (_startday != value)
                    {
                        _startday = value;
                        OnPropertyChanged(nameof(StartDayVM));
                    }
                }
            }
            private DateTime? _endtime;
            public DateTime? EndTimeVM
            {
                get { return _endtime; }
                set
                {
                    if (_endtime != value)
                    {
                        _endtime = value;
                        OnPropertyChanged(nameof(EndTimeVM));
                    }
                }
            }
            private DateTime? _endday;
            public DateTime? EndDayVM
            {
                get { return _endday; }
                set
                {
                    if (_endday != value)
                    {
                        _endday = value;
                        OnPropertyChanged(nameof(EndDayVM));
                    }
                }
            }
            private string _quality;
            public string QualityVM
            {
                get { return _quality; }
                set
                {
                    if (_quality != value)
                    {
                        _quality = value;
                        OnPropertyChanged(nameof(QualityVM));
                    }
                }
            }
            private string _type;
            public string TypeVM
            {
                get { return _type; }
                set
                {
                    if (_type != value)
                    {
                        _type = value;
                        OnPropertyChanged(nameof(TypeVM));
                    }
                }
            }
            public ICommand CancelCommand { get; set; }
            public ICommand ConfirmCommand { get; set; }
            public SleepTrackerViewModel() 
            {
                _sleepCollection = GetMongoCollectionFromSleep();

                ConfirmCommand = new RelayCommand<SleepTrackerView>(p => true, p => ConfirmCM(p));
                CancelCommand = new RelayCommand<SleepTrackerView>((p) => { return true; }, (p) => { p.Close(); });
            }  
            public void ConfirmCM(SleepTrackerView parameter)
            {
                if (ValidateInput(parameter))
                {
                    //Create new meal
                    Sleep newSleep = new Sleep
                    {
                        Username = Const.Instance.Username,
                        StartTime = StartTimeVM.HasValue ? StartTimeVM.Value.ToString("HH:mm") : null,
                        StartDay = StartDayVM.HasValue ? StartDayVM.Value.ToString("dd/MM/yyyy") : null,
                        EndTime = EndTimeVM.HasValue ? EndTimeVM.Value.ToString("HH:mm") : null,
                        EndDay = EndDayVM.HasValue ? EndDayVM.Value.ToString("dd/MM/yyyy") : null,
                        Type = TypeVM,
                        Quality = QualityVM
                    };
                    _sleepCollection.InsertOne(newSleep);


                MessageBox.Show("Add sleep successful!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    StartTimeVM = null;
                    StartDayVM = null;
                    EndTimeVM = null;
                    EndDayVM = null;
                    TypeVM = null;
                    QualityVM = null;
                    parameter.Close();
                }
            }
            private bool ValidateInput(SleepTrackerView parameter)
            {
                string starttime = StartTimeVM.HasValue ? StartTimeVM.Value.ToString("HH:mm") : null;
                if (string.IsNullOrEmpty(starttime))
                {
                    MessageBox.Show("Please enter the start time!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                    parameter.StartTime.Focus();
                    return false;
                }
                string startday = StartDayVM.HasValue ? StartDayVM.Value.ToString("dd/MM/yyyy") : null;
                if (string.IsNullOrEmpty(startday))
                {
                    MessageBox.Show("Please select the start day!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                    parameter.StartDay.Focus();
                    return false;
                }
                string endtime = EndTimeVM.HasValue ? EndTimeVM.Value.ToString("HH:mm") : null;
                if (string.IsNullOrEmpty(endtime))
                {
                    MessageBox.Show("Please select the end time!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                    parameter.EndTime.Focus();
                    return false;
                }
                string endday = EndDayVM.HasValue ? EndDayVM.Value.ToString("dd/MM/yyyy") : null;
                if (string.IsNullOrEmpty(endday))
                {
                    MessageBox.Show("Please enter the end day!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                    parameter.EndDay.Focus();
                    return false;
                }

                if (string.IsNullOrEmpty(TypeVM))
                {
                    MessageBox.Show("Please select the type!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                    parameter.Type.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(QualityVM))
                {
                    MessageBox.Show("Please select the quality!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                    parameter.Quality.Focus();
                    return false;
                }
                if (StartDayVM == EndDayVM)
                {
                    if (EndTimeVM <= StartTimeVM)
                    {
                        MessageBox.Show("Please enter the end time later than the start time!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                        parameter.StartTime.Focus();
                        return false;
                    }
                }
                if (StartDayVM > EndDayVM)
                {
                    MessageBox.Show("Please enter the end day later or equal than the start day!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                    parameter.StartDay.Focus();
                    return false;
                }
                return true;
            }
            private IMongoCollection<Sleep> GetMongoCollectionFromSleep()
            {
                // Set your MongoDB connection string and database name
                string connectionString = "mongodb+srv://HGB3009:HGB30092004@bao-database.xwrghva.mongodb.net/"; // Update with your MongoDB server details
                string databaseName = "HealthcareManagementDatabase"; // Update with your database name

                var client = new MongoClient(connectionString);
                var database = client.GetDatabase(databaseName);

                return database.GetCollection<Sleep>("Sleep");
            }
    }
    }
