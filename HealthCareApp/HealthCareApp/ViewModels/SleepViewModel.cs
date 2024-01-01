using HealthCareApp.Models;
using HealthCareApp.Views;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HealthCareApp.ViewModels
{
    public class SleepViewModel : BaseViewModel
    {
        private readonly IMongoCollection<Sleep> _sleepCollection;
        private string _viewby;
        public string ViewByVM
        {
            get { return _viewby; }
            set
            {
                if (_viewby != value)
                {
                    _viewby = value;
                    OnPropertyChanged(nameof(ViewByVM));
                }
            }
        }
        private DateTime? _viewbyday;
        public DateTime? ViewByDayVM
        {
            get { return _viewbyday; }
            set
            {
                if (_viewbyday != value)
                {
                    _viewbyday = value;
                    OnPropertyChanged(nameof(ViewByDayVM));
                }
            }
        }
        private string _viewbymonth;
        public string ViewByMonthVM
        {
            get { return _viewbymonth; }
            set
            {
                if (_viewbymonth != value)
                {
                    _viewbymonth = value;
                    OnPropertyChanged(nameof(ViewByMonthVM));
                }
            }
        }
        private string _viewbyyear;
        public string ViewByYearVM
        {
            get { return _viewbyyear; }
            set
            {
                if (_viewbyyear != value)
                {
                    _viewbyyear = value;
                    OnPropertyChanged(nameof(ViewByYearVM));
                }
            }
        }

        private ObservableCollection<Sleep> _sleeplist;
        public ObservableCollection<Sleep> SleepList
        {
            get { return _sleeplist; }
            private set
            {
                _sleeplist = value;
                OnPropertyChanged(nameof(SleepList));
            }
        }
        public ICommand ViewByChangeCM { get; set; }
        public ICommand ViewByDayChangeCM { get; set; }
        public ICommand ViewByMonthChangeCM { get; set; }
        public ICommand ViewByYearChangeCM { get; set; }
        public ICommand DeleteSleepCommand { get; set; }
        public ICommand SleepTrackerCommand { get; set; }
        public ICommand LoadWindowCommand { get; set; }
        public string ViewByMode { get; set; }
        public SleepViewModel()
        {
            _sleepCollection = GetMongoCollectionFromSleep();

            ViewByChangeCM = new RelayCommand<SleepView>((p) => true, (p) => ViewByChange(p));
            ViewByDayChangeCM = new RelayCommand<SleepView>((p) => true, (p) => ViewByDayChange(p));
            ViewByMonthChangeCM = new RelayCommand<SleepView>((p) => true, (p) => ViewByMonthChange(p));
            ViewByYearChangeCM = new RelayCommand<SleepView>((p) => true, (p) => ViewByYearChange(p));

            LoadWindowCommand = new RelayCommand<SleepView>((p) => true, (p) => LoadSleepList());
            SleepTrackerCommand = new RelayCommand<SleepView>((p) => { return true; }, (p) => SleepTracker(p));
            DeleteSleepCommand = new RelayCommand<Sleep>((sleep) => true, (sleep) => DeleteSleep(sleep));
        }
        public void ViewByChange(SleepView parameter)
        {
            if (!string.IsNullOrEmpty(ViewByVM))
            {
                if (ViewByVM == "Day")
                {
                    parameter.ChooseDay.Visibility = Visibility.Visible;
                    parameter.ChooseMonth.Visibility = Visibility.Hidden;
                    parameter.ChooseYear.Visibility = Visibility.Hidden;
                    ViewByMonthVM = null;
                    ViewByYearVM = null;
                    ViewByMode = null;
                }
                if (ViewByVM == "Month")
                {
                    parameter.ChooseDay.Visibility = Visibility.Hidden;
                    parameter.ChooseMonth.Visibility = Visibility.Visible;
                    parameter.ChooseYear.Visibility = Visibility.Hidden;
                    ViewByDayVM = null;
                    ViewByYearVM = null;
                    ViewByMode = null;
                }
                if (ViewByVM == "Year")
                {
                    parameter.ChooseDay.Visibility = Visibility.Hidden;
                    parameter.ChooseMonth.Visibility = Visibility.Hidden;
                    parameter.ChooseYear.Visibility = Visibility.Visible;
                    ViewByDayVM = null;
                    ViewByMonthVM = null;
                    ViewByMode = null;
                }
            }
            else
            {
                parameter.ChooseDay.Visibility = Visibility.Hidden;
                parameter.ChooseMonth.Visibility = Visibility.Hidden;
                parameter.ChooseYear.Visibility = Visibility.Hidden;
                ViewByDayVM = null;
                ViewByMonthVM = null;
                ViewByYearVM = null;
                ViewByMode = null;
                LoadSleepList();
            }
        }
        public void ViewByDayChange(SleepView parameter)
        {
            string day = ViewByDayVM.HasValue ? ViewByDayVM.Value.ToString("dd/MM/yyyy") : null;
            if (!string.IsNullOrEmpty(day))
            {
                ViewByMode = "Day";
            }
            else { ViewByMode = null; }
            LoadSleepList();
        }
        public void ViewByMonthChange(SleepView parameter)
        {
            if (!string.IsNullOrEmpty(ViewByMonthVM))
            {
                ViewByMode = "Month";
            }
            else { ViewByMode = null; }
            LoadSleepList();
        }
        public void ViewByYearChange(SleepView parameter)
        {
            if (!string.IsNullOrEmpty(ViewByYearVM))
            {
                ViewByMode = "Year";
            }
            else { ViewByMode = null; }
            LoadSleepList();
        }
        public void DeleteSleep(Sleep sleep)
        {
            if (sleep != null)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete this sleep process?",
                                                          "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var objectId = ObjectId.Parse(sleep.Id);
                    _sleepCollection.DeleteOne(Builders<Sleep>.Filter.Eq("_id", objectId));
                    LoadSleepList();
                }
            }
        }

        public void LoadSleepList()
        {
            var filterBuilder = Builders<Sleep>.Filter;
            var usernameFilter = filterBuilder.Eq(x => x.Username, Const.Instance.Username);

            FilterDefinition<Sleep> finalFilter = usernameFilter;

            if (!string.IsNullOrEmpty(ViewByMode))
            {
                if (ViewByMode == "Day")
                {
                    string searchDay = ViewByDayVM.HasValue ? ViewByDayVM.Value.ToString("dd/MM/yyyy") : null;

                    if (!string.IsNullOrEmpty(searchDay))
                    {
                        var startDayFilter = filterBuilder.Eq(x => x.StartDay, searchDay);
                        var endDayFilter = filterBuilder.Eq(x => x.EndDay, searchDay);

                        // Use Builders.Or to create an OR condition
                        finalFilter &= filterBuilder.Or(startDayFilter, endDayFilter);
                    }
                }
                else if (ViewByMode == "Month")
                {
                    int targetMonth = int.Parse(ViewByMonthVM);
                    string monthString = targetMonth.ToString("00");

                    finalFilter &= filterBuilder.Or(
                        filterBuilder.Regex(x => x.StartDay, new BsonRegularExpression($@"^.*\/{monthString}\/[0-9]{{4}}$")),
                        filterBuilder.Regex(x => x.EndDay, new BsonRegularExpression($@"^.*\/{monthString}\/[0-9]{{4}}$"))
                    );
                }
                else if (ViewByMode == "Year")
                {
                    int targetYear = int.Parse(ViewByYearVM);

                    finalFilter &= filterBuilder.Or(
                        filterBuilder.Regex(x => x.StartDay, new BsonRegularExpression($@"^.*\/[0-9]{{2}}\/{targetYear}$")),
                        filterBuilder.Regex(x => x.EndDay, new BsonRegularExpression($@"^.*\/[0-9]{{2}}\/{targetYear}$"))
                    );
                }
            }
            var sleeps = _sleepCollection.Find(finalFilter).ToList();
            SleepList = new ObservableCollection<Sleep>(sleeps);
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
        public void SleepTracker(SleepView parameter)
        {
            SleepTrackerView SleepTracker = new SleepTrackerView();
            parameter.Opacity = 0.5;
            SleepTracker.ShowDialog();
            parameter.Opacity = 1;
            LoadSleepList();
        }
    }
}