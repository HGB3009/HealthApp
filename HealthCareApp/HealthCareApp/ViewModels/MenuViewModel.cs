using HealthCareApp.Models;
using HealthCareApp.Views;
using LiveCharts;
using LiveCharts.Wpf;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace HealthCareApp.ViewModels
{
    public class MenuViewModel:BaseViewModel
    {
        private readonly IMongoCollection<Nutrition> _nutritionCollection;
        private readonly IMongoCollection<ExerciseLesson> _ExerciseCollection;
        public ICommand InitPieChartCommand { get; set; }
        private SeriesCollection pieSeriesCollection;
        public SeriesCollection PieSeriesCollection { get => pieSeriesCollection; set { pieSeriesCollection = value; OnPropertyChanged(); } }
        private Func<ChartPoint, string> labelPoint;
        public Func<ChartPoint, string> LabelPoint { get => labelPoint; set => labelPoint = value; }
        public MenuViewModel() 
        {
            _nutritionCollection = GetMongoCollectionFromNutrition();
            _ExerciseCollection = GetMongoCollectionFromExerciseLesson();

            InitPieChartCommand = new RelayCommand<MenuView>(parameter => true, parameter => InitPieChart(parameter));
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
                PieSeriesCollection.Clear();
            }
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
    }
}
