using HealthCareApp.Models;
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
    public class SleepHistoryViewModel : BaseViewModel
    {
        private readonly IMongoCollection<Sleep> _sleepCollection;
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
        public ICommand DeleteSleepCommand { get; set; }
        public SleepHistoryViewModel()
        {
            _sleepCollection = GetMongoCollectionFromSleep();

            DeleteSleepCommand = new RelayCommand<Sleep>((sleep) => true, (sleep) => DeleteSleep(sleep));
            var filterBuilder = Builders<Sleep>.Filter;
            var usernameFilter = filterBuilder.Eq(x => x.Username, Const.Instance.Username);
            var sleep = _sleepCollection.Find(usernameFilter).ToList();
            SleepList = new ObservableCollection<Sleep>(sleep);
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


            var sleep = _sleepCollection.Find(usernameFilter).ToList();
            SleepList = new ObservableCollection<Sleep>(sleep);
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
