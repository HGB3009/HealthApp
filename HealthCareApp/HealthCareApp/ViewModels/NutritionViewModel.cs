using HealthCareApp.Models;
using HealthCareApp.Views;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace HealthCareApp.ViewModels
{
    
    public class NutritionViewModel : BaseViewModel
    {
        
        private ObservableCollection<Nutrition> _meallist;
        public ObservableCollection<Nutrition> MealList
        {
            get { return _meallist; }
            private set
            {
                _meallist = value;
                OnPropertyChanged(nameof(MealList));
            }
        }
        private readonly IMongoCollection<Nutrition> _nutritionCollection;
        public ICommand AddMealCommand { get; set; }
        public ICommand CalculateCommand { get; set; }
        public ICommand LoadWindowCommand { get; set; }
        public NutritionViewModel()
        {
            _nutritionCollection = GetMongoCollectionFromNutrition();
            LoadWindowCommand = new RelayCommand<NutritionView>((p) => true, (p) => LoadMenuItem(p));
            AddMealCommand = new RelayCommand<NutritionView>((p) => { return true; }, (p) => AddMeal(p));
            CalculateCommand = new RelayCommand<NutritionView>((p) => { return true; }, (p) => Calculate(p));
        }
        private void LoadMenuItem(NutritionView parameter)
        {
            var filter = Builders<Nutrition>.Filter.Eq(x => x.Username, Const.Instance.Username);

            var meals = _nutritionCollection.Find(filter).ToList();

            // Gán danh sách meals cho MealList
            MealList = new ObservableCollection<Nutrition>(meals);
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
        public void AddMeal(NutritionView parameter)
        {
            AddMealView addMealView = new AddMealView();
            parameter.Opacity = 0.5;
            addMealView.ShowDialog();
            parameter.Opacity = 1;
        }
        public void Calculate(NutritionView parameter)
        {
            CalculateCaloriesView calculateCaloriesView = new CalculateCaloriesView();
            parameter.Opacity = 0.5;
            calculateCaloriesView.ShowDialog();
            parameter.Opacity = 1;
        }
    }
}
