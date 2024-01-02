using HealthCareApp.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using MongoDB.Bson;
using HealthCareApp.Views;
using System.Reflection.Metadata;

namespace HealthCareApp.ViewModels
{
    public class EditExerciseListViewModel : BaseViewModel
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
        public ExerciseLesson excercise;
        public ExerciseLesson Exercise { get { return excercise; } set { excercise = value; OnPropertyChanged(nameof(Exercise)); } }

        public ICommand ExitBtnCommand { get; set; }
        public ICommand ConfirmBtnCommand { get; set; }
        public ICommand LoadWindowCommand { get; set; }

        private readonly IMongoCollection<ExerciseLesson> _ExerciseCollection;
        public EditExerciseListViewModel()
        {        }
        public EditExerciseListViewModel(ExerciseLesson ex)
        {
            _ExerciseCollection = GetMongoCollectionFromExerciseLesson();
            Exercise = ex;
            _LoadWindow();

             ConfirmBtnCommand = new RelayCommand<EditExerciseListView>((p) => true, (p) => EditExerciseCM(p));
            ExitBtnCommand = new RelayCommand<EditExerciseListView>(p => true, p => CancelCM(p));
        }
        private void EditExerciseCM(EditExerciseListView p)
        {
            bool isEdit = false;
            var objectId = ObjectId.Parse(Exercise.Id);
            var filter = Builders<ExerciseLesson>.Filter.Eq("Id", objectId);
            var ex = _ExerciseCollection.Find(filter).FirstOrDefault();

            if (NameExercise != ex.ExerciseName)
            {
                ex.ExerciseName = NameExercise;

                var updateDefinition = Builders<ExerciseLesson>.Update.Set(u => u.ExerciseName, NameExercise);

                _ExerciseCollection.UpdateOne(filter, updateDefinition);
                isEdit = true;
            }
            if (TypeExercise != ex.ExerciseType)
            {
                ex.ExerciseType = TypeExercise;

                var updateDefinition = Builders<ExerciseLesson>.Update.Set(u => u.ExerciseType, TypeExercise);

                _ExerciseCollection.UpdateOne(filter, updateDefinition);
                isEdit = true;
            }
            if (CaloriesExercise != ex.Calories.ToString())
            {
                double temp = double.Parse(CaloriesExercise);
                ex.Calories = temp;

                var updateDefinition = Builders<ExerciseLesson>.Update.Set(u => u.Calories, temp);

                _ExerciseCollection.UpdateOne(filter, updateDefinition);
                isEdit = true;
            }
            if (EquipmentExercise != ex.Equipment)
            {
                ex.Equipment = EquipmentExercise;

                var updateDefinition = Builders<ExerciseLesson>.Update.Set(u => u.Equipment, EquipmentExercise);

                _ExerciseCollection.UpdateOne(filter, updateDefinition);
                isEdit = true;
            }
            string day = DayExercise.HasValue ? DayExercise.Value.ToString("dd/MM/yyyy") : null;
            if (day != ex.ExerciseDay)
            {
                ex.ExerciseDay = day;

                var updateDefinition = Builders<ExerciseLesson>.Update.Set(u => u.ExerciseDay, day);

                _ExerciseCollection.UpdateOne(filter, updateDefinition);
                isEdit = true;
            }
            string starttime = TimeStart.HasValue ? TimeStart.Value.ToString("HH/mm") : null;
            if (starttime != ex.ExerciseTimeStart)
            {
                ex.ExerciseTimeStart = starttime;

                var updateDefinition = Builders<ExerciseLesson>.Update.Set(u => u.ExerciseTimeStart, starttime);

                _ExerciseCollection.UpdateOne(filter, updateDefinition);
                isEdit = true;
            }
            string endtime = TimeEnd.HasValue ? TimeEnd.Value.ToString("HH/mm") : null;
            if (endtime != ex.ExerciseTimeEnd)
            {
                ex.ExerciseTimeEnd = endtime;

                var updateDefinition = Builders<ExerciseLesson>.Update.Set(u => u.ExerciseTimeEnd, endtime);

                _ExerciseCollection.UpdateOne(filter, updateDefinition);
                isEdit = true;
            }
            if (isEdit)
            {
                MessageBox.Show("Information changed successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Nothing changed!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void CancelCM(EditExerciseListView p)
        {
            if (p != null)
            {
                _LoadWindow();
            }
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
        public void _LoadWindow()
        {
            var objectId = ObjectId.Parse(Exercise.Id);
            var filter = Builders<ExerciseLesson>.Filter.Eq("Id", objectId);
            var ex = _ExerciseCollection.Find(filter).FirstOrDefault();

            if (ex != null)
            {
                Exercise = ex;
                NameExercise = ex.ExerciseName;
                TypeExercise = ex.ExerciseType;
                DateTime starttime;
                if (DateTime.TryParseExact(ex.ExerciseTimeStart, "HH/mm", null, System.Globalization.DateTimeStyles.None, out starttime))
                {
                    TimeStart = starttime;
                }
                DateTime endtime;
                if (DateTime.TryParseExact(ex.ExerciseTimeEnd, "HH/mm", null, System.Globalization.DateTimeStyles.None, out endtime))
                {
                    TimeEnd = endtime;
                }

                DateTime exday;
                if (DateTime.TryParseExact(ex.ExerciseDay, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out exday))
                {
                    DayExercise = exday;
                }
                CaloriesExercise = ex.Calories.ToString();
                EquipmentExercise = ex.Equipment;

            }
        }
    }
}
