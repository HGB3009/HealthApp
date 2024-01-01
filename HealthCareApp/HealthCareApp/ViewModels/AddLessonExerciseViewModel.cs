using HealthCareApp.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZstdSharp.Unsafe;

namespace HealthCareApp.ViewModels
{
    public class AddLessonExerciseViewModel : BaseViewModel
    {
        public string nameExercise;
        public string NameExercise { get { return nameExercise; } set { nameExercise = value; OnPropertyChanged(nameof(NameExercise)); } }

        public string typeExercise;
        public string TypeExercise { get { return typeExercise; } set { typeExercise = value; OnPropertyChanged(nameof(TypeExercise)); } }

        public DateTime? timeStart;
        public DateTime? TimeStart { get { return timeStart; } set { timeStart = value; OnPropertyChanged(nameof(TimeStart)); } }

        public DateTime? timeEnd;
        public DateTime? TimeEnd { get { return timeEnd; } set { timeEnd = value; OnPropertyChanged(nameof(TimeEnd)); } }

        public DateTime? dayExercise;
        public DateTime? DayExercise { get { return dayExercise; } set { dayExercise = value; OnPropertyChanged(nameof(DayExercise)); } }

        public string caloriesExercise;
        public string CaloriesExercise { get { return caloriesExercise; } set { caloriesExercise = value; OnPropertyChanged(nameof(CaloriesExercise)); } }

        public string equipmentExercise;
        public string EquipmentExercise { get { return equipmentExercise; } set { equipmentExercise = value; OnPropertyChanged(nameof(EquipmentExercise)); } }

        public ICommand ExitBtnCommand { get; set; }
        public ICommand AddBtnCommand { get; set; }

        private readonly IMongoCollection<ExerciseLesson> _ExerciseCollection;
        public AddLessonExerciseViewModel()
        {
            _ExerciseCollection = GetMongoCollectionFromExerciseLesson();
            ExitBtnCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { ShowNullView(); p.Close(); });
            AddBtnCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddLessonExercise();
            });
        }
        private void AddLessonExercise()
        {
            if (Validation())
            {
                ExerciseLesson newLesson = new ExerciseLesson
                {
                    Username = Const.Instance.Username,
                    ExerciseName = NameExercise,
                    ExerciseType = TypeExercise,
                    ExerciseTimeEnd = TimeEnd.Value.TimeOfDay,
                    ExerciseTimeStart = TimeStart.Value.TimeOfDay,
                    ExerciseDay = DayExercise.Value.ToString("dd/MM/yyyy"),
                    Equipment = EquipmentExercise,
                    Calories = CaloriesExercise,
                };
                _ExerciseCollection.InsertOne(newLesson);
                MessageBox.Show("Add exercise lesson successful!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                ShowNullView();
            }
        }

        private bool Validation()
        {
            if (string.IsNullOrEmpty(NameExercise))
            {
                MessageBox.Show("Please enter the Name field");
                return false;
            }
            if (string.IsNullOrEmpty(TypeExercise))
            {
                MessageBox.Show("Please enter the Type field");
                return false;
            }
            string timeStart = TimeStart.HasValue ? TimeStart.Value.ToString() : null;
            if (string.IsNullOrEmpty(timeStart))
            {
                MessageBox.Show("Please enter the Time Start field.");
                return false;
            }
            string timeEnd = TimeEnd.HasValue ? TimeEnd.Value.ToString() : null;
            if (string.IsNullOrEmpty(timeEnd))
            {
                MessageBox.Show("Please enter the Time End field.");
                return false;
            }
            string day = DayExercise.HasValue ? DayExercise.Value.ToString() : null;
            if (string.IsNullOrEmpty(day))
            {
                MessageBox.Show("Please enter the Day field");
                return false;
            }
            if (string.IsNullOrEmpty(CaloriesExercise.ToString()))
            {
                MessageBox.Show("Please enter the Name field");
                return false;
            }
            if (string.IsNullOrEmpty(EquipmentExercise))
            {
                MessageBox.Show("Please enter the Equipment field");
                return false;
            }
            return true;
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
        private void ShowNullView()
        {
            NameExercise = null;
            TypeExercise = null;
            TimeEnd = null;
            TimeStart = null;
            DayExercise = null;
            EquipmentExercise = null;
            CaloriesExercise = null;
        }
    }
}
