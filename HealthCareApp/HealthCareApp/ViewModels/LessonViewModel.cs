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
    public class LessonViewModel:BaseViewModel
    {
        public ObservableCollection<ExerciseLesson> exerciseList;
        public ObservableCollection<ExerciseLesson> ExerciseList
        {
            get { return exerciseList; }
            set
            {
                exerciseList = value;
                OnPropertyChanged(nameof(ExerciseList));
            }
        }
        public ICommand DeleteExerciseCommand {  get; set; }
        public ICommand RefreshBtnCommand { get; set; }
        private readonly IMongoCollection<ExerciseLesson> _collectionExercise;
        public LessonViewModel()
        {
            _collectionExercise = GetMongoCollectionFromExercise();
            DeleteExerciseCommand = new RelayCommand<ExerciseLesson>((p) => { return true; }, (p) => { DeleteExercise(p); });
            RefreshBtnCommand = new RelayCommand<object>((p) => { return true; }, (p) => { LoadExerciseList(); });
            LoadExerciseList();
        }

        public void LoadExerciseList()
        {
            var filterBuilder = Builders<ExerciseLesson>.Filter;
            var usernameFilter = filterBuilder.Eq(x => x.Username, Const.Instance.Username);

            FilterDefinition<ExerciseLesson> finalFilter = usernameFilter;

            var exercises = _collectionExercise.Find(finalFilter).ToList();
            ExerciseList = new ObservableCollection<ExerciseLesson>(exercises);
        }
        private IMongoCollection<ExerciseLesson> GetMongoCollectionFromExercise()
        {
            // Set your MongoDB connection string and database name
            string connectionString = "mongodb+srv://HGB3009:HGB30092004@bao-database.xwrghva.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "HealthcareManagementDatabase"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<ExerciseLesson>("ExerciseLesson");
        }
        public void DeleteExercise(ExerciseLesson exercise)
        {
            if (exercise != null)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {exercise.ExerciseName}?",
                                                          "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var objectId = ObjectId.Parse(exercise.Id);
                    _collectionExercise.DeleteOne(Builders<ExerciseLesson>.Filter.Eq("_id", objectId));
                    LoadExerciseList();
                }
            }
        }
    }
}
