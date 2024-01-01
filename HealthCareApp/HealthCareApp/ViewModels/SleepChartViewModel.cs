using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCareApp.Models;
using MongoDB.Driver;

namespace HealthCareApp.ViewModels
{
    public class SleepChartViewModel : BaseViewModel
    {
        private readonly IMongoCollection<SleepPerDay> _sleepPerDayCollection;

        public SleepChartViewModel()
        {
            _sleepPerDayCollection = GetMongoCollectionFromSleepPerDay();
            LoadChartData();
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }

        private void LoadChartData()
        {
            var someFilter = Builders<SleepPerDay>.Filter.Eq(s => s.Username, Const.Instance.Username);
            var sleepData = _sleepPerDayCollection.Find(someFilter).ToList(); 

            SeriesCollection = new SeriesCollection
        {
            new ColumnSeries
            {
                Title = "Sleep time",
                Values = new ChartValues<double>(sleepData.Select(s => s.Hour))
            }
        };

            Labels = sleepData.Select(s => s.Day).ToArray();
        }
        private IMongoCollection<SleepPerDay> GetMongoCollectionFromSleepPerDay()
        {
            // Set your MongoDB connection string and database name
            string connectionString = "mongodb+srv://HGB3009:HGB30092004@bao-database.xwrghva.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "HealthcareManagementDatabase"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<SleepPerDay>("SleepPerDay");
        }
    }
}
