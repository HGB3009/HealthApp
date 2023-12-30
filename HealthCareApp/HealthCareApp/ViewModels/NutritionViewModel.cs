using HealthCareApp.Models;
using HealthCareApp.Views;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HealthCareApp.ViewModels
{

    public class NutritionViewModel : BaseViewModel
    {
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
        public ICommand ViewByChangeCM { get; set; }
        public ICommand ViewByDayChangeCM { get; set; }
        public ICommand ViewByMonthChangeCM { get; set; }
        public ICommand ViewByYearChangeCM { get; set; }
        public ICommand AddMealCommand { get; set; }
        public ICommand CalculateCommand { get; set; }
        public ICommand LoadWindowCommand { get; set; }
        public string ViewByMode { get; set; }
        public NutritionViewModel()
        {
            _nutritionCollection = GetMongoCollectionFromNutrition();
            ViewByChangeCM = new RelayCommand<NutritionView>((p) => true, (p) => ViewByChange(p));
            ViewByDayChangeCM = new RelayCommand<NutritionView>((p) => true, (p) => ViewByDayChange(p));
            ViewByMonthChangeCM = new RelayCommand<NutritionView>((p) => true, (p) => ViewByMonthChange(p));
            ViewByYearChangeCM = new RelayCommand<NutritionView>((p) => true, (p) => ViewByYearChange(p));
            LoadWindowCommand = new RelayCommand<NutritionView>((p) => true, (p) => LoadMealList(p));
            AddMealCommand = new RelayCommand<NutritionView>((p) => { return true; }, (p) => AddMeal(p));
            CalculateCommand = new RelayCommand<NutritionView>((p) => { return true; }, (p) => Calculate(p));
        }
        public void ViewByChange(NutritionView parameter)
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
                LoadMealList(parameter);
            }
        }
        public void ViewByDayChange(NutritionView parameter)
        {
            string day = ViewByDayVM.HasValue ? ViewByDayVM.Value.ToString("dd/MM/yyyy") : null;
            if (!string.IsNullOrEmpty(day))
            {
                ViewByMode = "Day";
            }
            else { ViewByMode = null; }
            LoadMealList(parameter);
        }
        public void ViewByMonthChange(NutritionView parameter)
        {
            if (!string.IsNullOrEmpty(ViewByMonthVM))
            {
                ViewByMode = "Month";
            }
            else { ViewByMode = null; }
            LoadMealList(parameter);
        }
        public void ViewByYearChange(NutritionView parameter)
        {
            if (!string.IsNullOrEmpty(ViewByYearVM))
            {
                ViewByMode = "Year";
            }
            else { ViewByMode = null; }
            LoadMealList(parameter);
        }
        public void LoadMealList(NutritionView parameter)
        {
            var filterBuilder = Builders<Nutrition>.Filter;
            var usernameFilter = filterBuilder.Eq(x => x.Username, Const.Instance.Username);

            FilterDefinition<Nutrition> finalFilter = usernameFilter;

            if (!string.IsNullOrEmpty(ViewByMode))
            {

                if (ViewByMode == "Day")
                {
                    string SreachDay = ViewByDayVM.HasValue ? ViewByDayVM.Value.ToString("dd/MM/yyyy") : null;
                    finalFilter &= filterBuilder.Eq(x => x.Day, SreachDay);
                }
                else if (ViewByMode == "Month")
                {
                    int targetMonth = int.Parse(ViewByMonthVM);
                    string monthString = targetMonth.ToString("00");

                    finalFilter &= filterBuilder.Regex(
                        x => x.Day,
                        new BsonRegularExpression($@"^.*\/{monthString}\/[0-9]{{4}}$")
                    );
                }
                else if (ViewByMode == "Year")
                {
                    int targetYear = int.Parse(ViewByYearVM);
                    finalFilter &= filterBuilder.Regex(
                        x => x.Day,
                        new BsonRegularExpression($@"^.*\/[0-9]{{2}}\/{targetYear}$")
                    );
                }
            }

            var meals = _nutritionCollection.Find(finalFilter).ToList();
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
            LoadMealList(parameter);
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
