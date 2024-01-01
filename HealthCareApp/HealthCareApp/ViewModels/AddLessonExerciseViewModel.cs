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

        public DateTime? dayStart;
        public DateTime? DayStart { get { return dayStart; } set { dayStart = value; OnPropertyChanged(nameof(DayStart)); } }

        public DateTime? dayEnd;
        public DateTime? DayEnd { get { return dayEnd; } set { dayEnd = value; OnPropertyChanged(nameof(DayEnd)); } }

        public string timeExercise;
        public string TimeExercise { get { return timeExercise; } set { timeExercise = value; OnPropertyChanged(nameof(TimeExercise)); } }

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
            ExitBtnCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
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
                    ExerciseType=TypeExercise,
                    ExerciseDayEnd=DayEnd,
                    ExerciseDayStart=DayStart,
                    ExerciseTime=TimeExercise,
                    Equipment=EquipmentExercise,
                    Calories=CaloriesExercise,
                };
                _ExerciseCollection.InsertOne(newLesson);
                MessageBox.Show("Add exercise lesson successful!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                NameExercise = null;
                TypeExercise = null;
                DayEnd = null;
                DayStart = null;
                TimeExercise = null;
                EquipmentExercise = null;
                CaloriesExercise = null;
            }
        }
    
    private bool Validation()
    {
        if (string.IsNullOrEmpty(EquipmentExercise))
        {
            MessageBox.Show("Please enter the Equipment field");
            return false;
        }
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
        if (string.IsNullOrEmpty(TimeExercise))
        {
            MessageBox.Show("Please enter the Time field");
            return false;
        }
        if (string.IsNullOrEmpty(CaloriesExercise.ToString()))
        {
            MessageBox.Show("Please enter the Name field");
            return false;
        }
        string dayStart = DayStart.HasValue ? DayStart.Value.ToString() : null;
        if (string.IsNullOrEmpty(dayStart))
        {
            MessageBox.Show("Please enter the Day Start field.");
            return false;
        }
        string dayEnd = DayEnd.HasValue ? DayEnd.Value.ToString() : null;
        if (string.IsNullOrEmpty(dayEnd))
        {
            MessageBox.Show("Please enter the Day End field.");
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
}
}
