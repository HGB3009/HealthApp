using HealthCareApp.Models;
using HealthCareApp.Views;
using LiveCharts;
using LiveCharts.Wpf;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace HealthCareApp.ViewModels
{
    public class MenuViewModel:BaseViewModel
    {
        private readonly IMongoCollection<Sleep> _sleepCollection;
        private readonly IMongoCollection<Nutrition> _nutritionCollection;
        private readonly IMongoCollection<ExerciseLesson> _ExerciseCollection;
        private readonly IMongoCollection<WaterPerDay> _waterperdayCollection;
        private SeriesCollection pieSeriesCollection;
        public SeriesCollection PieSeriesCollection { get => pieSeriesCollection; set { pieSeriesCollection = value; OnPropertyChanged(); } }
        private Func<ChartPoint, string> labelPoint;
        public Func<ChartPoint, string> LabelPoint { get => labelPoint; set => labelPoint = value; }
        private SeriesCollection waterpieSeriesCollection;
        public SeriesCollection WaterPieSeriesCollection { get => pieSeriesCollection; set { pieSeriesCollection = value; OnPropertyChanged(); } }
        private Func<ChartPoint, string> waterlabelPoint;
        public Func<ChartPoint, string> WaterLabelPoint { get => waterlabelPoint; set => waterlabelPoint = value; }
        private string _amountofwater;
        public string AmountOfWaterVM
        {
            get { return _amountofwater; }
            set
            {
                if (_amountofwater != value)
                {
                    _amountofwater = value;
                    OnPropertyChanged(nameof(AmountOfWaterVM));
                }
            }
        }
        private string _totalwater;
        public string TotalVM
        {
            get { return _totalwater; }
            set
            {
                if (_totalwater != value)
                {
                    _totalwater = value;
                    OnPropertyChanged(nameof(TotalVM));
                }
            }
        }
        private string _lastsleepduration;
        public string LastSleepDurationVM
        {
            get { return _lastsleepduration; }
            set
            {
                if (_lastsleepduration != value)
                {
                    _lastsleepduration = value;
                    OnPropertyChanged(nameof(LastSleepDurationVM));
                }
            }
        }
        public ICommand InitPieChartCommand { get; set; }
        public ICommand InitWaterChartCommand { get; set; }
        public ICommand UpdateBodyCommand { get; set; }
        public ICommand LoadWindowCommand { get; set; }
        public ICommand AddingWaterCommand { get; set; }
        public MenuViewModel() 
        {
            _nutritionCollection = GetMongoCollectionFromNutrition();
            _ExerciseCollection = GetMongoCollectionFromExerciseLesson();
            _waterperdayCollection = GetMongoCollectionFromWaterPerDay();
            _sleepCollection = GetMongoCollectionFromSleep();

            InitPieChartCommand = new RelayCommand<MenuView>(parameter => true, parameter => InitPieChart(parameter));
            InitWaterChartCommand = new RelayCommand<MenuView>(parameter => true, parameter => InitWaterChart(parameter));

            UpdateBodyCommand = new RelayCommand<MenuView>((p) => true, (p) => UpdateBodyCM(p));
            AddingWaterCommand = new RelayCommand<MenuView>((p) => true, (p) => AddingWaterCM(p));
            LoadWindowCommand = new RelayCommand<MenuView>((p) => true, (p) => _LoadWindow(p));
        }
        public void UpdateBodyCM(MenuView view)
        {
            UpdateBodyView updateBodyView = new UpdateBodyView();
            view.Opacity = 0.5;
            updateBodyView.ShowDialog();
            view.Opacity = 1;
            _LoadWindow(view);
        }
        public Sleep GetNearestSleep(List<Sleep> sleeps)
        {
            DateTime now = DateTime.Now;

            Sleep nearestSleep = sleeps.OrderBy(sleep =>
            {
                DateTime sleepStartTime = DateTime.ParseExact(sleep.StartDay + " " + sleep.StartTime, "dd/MM/yyyy HH:mm", null);
                DateTime sleepEndTime = DateTime.ParseExact(sleep.EndDay + " " + sleep.EndTime, "dd/MM/yyyy HH:mm", null);

                return Math.Min(Math.Abs((now - sleepStartTime).TotalSeconds), Math.Abs((now - sleepEndTime).TotalSeconds));
            }).FirstOrDefault();

            return nearestSleep;
        }
        public void AddingWaterCM(MenuView view)
        {
            string username = Const.Instance.Username;
            string currentDay = DateTime.Now.ToString("dd/MM/yyyy");
            double amount = Convert.ToDouble(AmountOfWaterVM);

            var filter = Builders<WaterPerDay>.Filter.And(
                Builders<WaterPerDay>.Filter.Eq(x => x.Username, username),
                Builders<WaterPerDay>.Filter.Eq(x => x.Day, currentDay)
            );

            var update = Builders<WaterPerDay>.Update.Inc(x => x.AmountOfWater, amount);

            var options = new FindOneAndUpdateOptions<WaterPerDay, WaterPerDay>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

            var result = _waterperdayCollection.FindOneAndUpdate(filter, update, options);

            if (result == null)
            {
                _waterperdayCollection.InsertOne(new WaterPerDay
                {
                    Username = username,
                    Day = currentDay,
                    AmountOfWater = amount
                });
            }
            _LoadWindow(view);
        }
        public void InitWaterChart(MenuView homeWindow)
        {
            string username = Const.Instance.Username;
            var filterBuilder = Builders<WaterPerDay>.Filter;
            var usernameFilter = filterBuilder.Eq(x => x.Username, Const.Instance.Username);
            FilterDefinition<WaterPerDay> finalFilter = usernameFilter;
            string currentDay = DateTime.Now.ToString("dd/MM/yyyy");
            finalFilter &= filterBuilder.Eq(x => x.Day, currentDay);

            var amountwater = _waterperdayCollection.Find(finalFilter).FirstOrDefault();
            waterlabelPoint = chartPoint => string.Format("{0:N0}", chartPoint.Y);
            if (amountwater != null)
            {
                if(amountwater.AmountOfWater > 2)
                {
                    WaterPieSeriesCollection = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title = "Drink",
                        Values = new ChartValues<double> { amountwater.AmountOfWater },
                        Fill = (Brush)new BrushConverter().ConvertFrom("Blue"),
                        DataLabels = true,
                        FontSize = 16,
                    },
                };
                }
                else
                {
                    double leftwater = 2 - amountwater.AmountOfWater;
                    WaterPieSeriesCollection = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title = "Drink",
                        Values = new ChartValues<double> { amountwater.AmountOfWater },
                        Fill = (Brush)new BrushConverter().ConvertFrom("Blue"),
                        DataLabels = true,
                        FontSize = 16,
                    },
                    new PieSeries
                    {
                        Title="Not Drink",
                        Values = new ChartValues<double> { leftwater },
                        Fill = (Brush)new BrushConverter().ConvertFrom("Gray"),
                        DataLabels = true,
                        FontSize = 16,
                    },
                };
                }
            }
        }
        public void InitPieChart(MenuView homeWindow)
        {
            string username = Const.Instance.Username;

            var filterBuilder1 = Builders<Nutrition>.Filter;
            var usernameFilter1 = filterBuilder1.Eq(x => x.Username, Const.Instance.Username);
            FilterDefinition<Nutrition> finalFilter1 = usernameFilter1;

            var filterBuilder2 = Builders<ExerciseLesson>.Filter;
            var usernameFilter2 = filterBuilder2.Eq(x => x.Username, Const.Instance.Username);
            FilterDefinition<ExerciseLesson> finalFilter2 = usernameFilter2;

            labelPoint = chartPoint => string.Format("{0:N0}", chartPoint.Y);
            if (homeWindow.SelectTimeCalories.SelectedIndex == 0)
            {
                string currentDay = DateTime.Now.ToString("dd/MM/yyyy");
                finalFilter1 &= filterBuilder1.Eq(x => x.Day, currentDay);
                finalFilter2 &= filterBuilder2.Eq(x => x.ExerciseDay, currentDay);

                var meals = _nutritionCollection.Find(finalFilter1).ToList();
                double eatenCalories = meals.Sum(meal => meal.Calories);

                var exercise = _ExerciseCollection.Find(finalFilter2).ToList();
                double burnCalories = exercise.Sum(exercise => exercise.Calories);

                PieSeriesCollection = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title = "Burn",
                        Values = new ChartValues<double> { burnCalories },
                        Fill = (Brush)new BrushConverter().ConvertFrom("#FF7222"),
                        DataLabels = true,
                        FontSize = 16,
                        LabelPoint = labelPoint,
                    },
                    new PieSeries
                    {
                        Title="Eaten",
                        Values = new ChartValues<double> { eatenCalories },
                        Fill = (Brush)new BrushConverter().ConvertFrom("Green"),
                        DataLabels = true,
                        FontSize = 16,
                        LabelPoint = labelPoint,
                    },
                };
            }
            else if (homeWindow.SelectTimeCalories.SelectedIndex == 1)
            {
                string currentMonth = DateTime.Now.Month.ToString("00");
                int currentYear = DateTime.Now.Year;
                finalFilter1 &= filterBuilder1.Regex(
                        x => x.Day,
                        new BsonRegularExpression($@"^.*\/{currentMonth}\/{currentYear}$")
                    );
                finalFilter2 &= filterBuilder2.Regex(
                        x => x.ExerciseDay,
                        new BsonRegularExpression($@"^.*\/{currentMonth}\/{currentYear}$")
                    );

                var meals = _nutritionCollection.Find(finalFilter1).ToList();
                double eatenCalories = meals.Sum(meal => meal.Calories);

                var exercise = _ExerciseCollection.Find(finalFilter2).ToList();
                double burnCalories = exercise.Sum(exercise => exercise.Calories);

                PieSeriesCollection = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title = "Burn",
                        Values = new ChartValues<double> { burnCalories },
                        Fill = (Brush)new BrushConverter().ConvertFrom("Orange"),
                        DataLabels = true,
                        FontSize = 16,
                        LabelPoint = labelPoint,
                    },
                    new PieSeries
                    {
                        Title="Eaten",
                        Values = new ChartValues<double> { eatenCalories },
                        Fill = (Brush)new BrushConverter().ConvertFrom("Green"),
                        DataLabels = true,
                        FontSize = 16,
                        LabelPoint = labelPoint,
                    },
                };
            }
            else
            {
                if(PieSeriesCollection != null)
                {
                    PieSeriesCollection.Clear();
                }

            }
        }
        void _LoadWindow(MenuView p)
        {
            string username = Const.Instance.Username;

            var filter = Builders<WaterPerDay>.Filter.Eq(x => x.Username, username);
            var User = _waterperdayCollection.Find(filter).FirstOrDefault();

            if (User != null)
            {
                InitPieChart(p);
                InitWaterChart(p);
                TotalVM = User.AmountOfWater.ToString() + " L";
            }

            var filter1 = Builders<Sleep>.Filter.Eq(x => x.Username, username);
            var sleeps = _sleepCollection.Find(filter1).ToList();
            var sleep = GetNearestSleep(sleeps);
            LastSleepDurationVM = sleep.SleepTime;
        }
        private IMongoCollection<Nutrition> GetMongoCollectionFromNutrition()
        {
            // Set your MongoDB connection string and database name
            string connectionString = "mongodb+srv://HGB3009:HGB30092004@bao-database.xwrghva.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "HealthcareManagementDatabase"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Nutrition>("Nutrition");
        }
        private IMongoCollection<ExerciseLesson> GetMongoCollectionFromExerciseLesson()
        {
            // Set your MongoDB connection string and database name
            string connectionString = "mongodb+srv://HGB3009:HGB30092004@bao-database.xwrghva.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "HealthcareManagementDatabase"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<ExerciseLesson>("ExerciseLesson");
        }
        private IMongoCollection<WaterPerDay> GetMongoCollectionFromWaterPerDay()
        {
            // Set your MongoDB connection string and database name
            string connectionString = "mongodb+srv://HGB3009:HGB30092004@bao-database.xwrghva.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "HealthcareManagementDatabase"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<WaterPerDay>("WaterPerDay");
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
