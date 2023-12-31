using HealthCareApp.Models;
using HealthCareApp.Views;
using Microsoft.Win32;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace HealthCareApp.ViewModels
{
    public class EditMealViewModel : BaseViewModel
    {
        private readonly IMongoCollection<Nutrition> _nutritionCollection;
        private string _username;
        public string UserNameVM
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(UserNameVM));
                }
            }
        }
        private string _mealtime;
        public string MealTimeVM
        {
            get { return _mealtime; }
            set
            {
                if (_mealtime != value)
                {
                    _mealtime = value;
                    OnPropertyChanged(nameof(MealTimeVM));
                }
            }
        }
        private string _mealname;
        public string MealNameVM
        {
            get { return _mealname; }
            set
            {
                if (_mealname != value)
                {
                    _mealname = value;
                    OnPropertyChanged(nameof(MealNameVM));
                }
            }
        }
        private DateTime? _day;
        public DateTime? DayVM
        {
            get { return _day; }
            set
            {
                if (_day != value)
                {
                    _day = value;
                    OnPropertyChanged(nameof(DayVM));
                }
            }
        }
        private string _quantity;
        public string QuantityVM
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(QuantityVM));
                }
            }
        }
        private string _unit;
        public string UnitVM
        {
            get { return _unit; }
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    OnPropertyChanged(nameof(UnitVM));
                }
            }
        }
        private string _calories;
        public string CaloriesVM
        {
            get { return _calories; }
            set
            {
                if (_calories != value)
                {
                    _calories = value;
                    OnPropertyChanged(nameof(CaloriesVM));
                }
            }
        }
        private byte[] _mealpicture;
        public byte[] MealPictureVM
        {
            get { return _mealpicture; }
            set
            {
                if (_mealpicture != value)
                {
                    _mealpicture = value;
                    OnPropertyChanged(nameof(MealPictureVM));
                }
            }
        }
        private Nutrition _meal;
        public Nutrition Meal
        {
            get { return _meal; }
            set
            {
                if (_meal != value)
                {
                    _meal = value;
                    OnPropertyChanged(nameof(Meal));
                }
            }
        }
        private BitmapImage _mealPicturesource;
        public BitmapImage MealPictureSourceVM
        {
            get => _mealPicturesource;
            set
            {
                _mealPicturesource = value;
                OnPropertyChanged(nameof(MealPictureSourceVM));
            }
        }
        public ICommand CancelEditMealCommand { get; set; }
        public ICommand BrowseImageCommand { get; set; }
        public ICommand ComfirmEditMealCommand { get; set; }
        public ICommand LoadWindowCommand { get; set; }
        public EditMealViewModel(){}
        public EditMealViewModel(Nutrition meal)
        {
            _nutritionCollection = GetMongoCollectionFromNutrition();
            LoadWindowCommand = new RelayCommand<EditMealView>((p) => true, (p) => _LoadWindow());
            Meal = meal;
            UserNameVM = meal.Username;
            MealTimeVM = meal.MealTime;
            MealNameVM = meal.MealName;
            DateTime parsedDay;
            if (DateTime.TryParseExact(meal.Day, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDay))
            {
                DayVM = parsedDay;
            }
            UnitVM = meal.Unit;
            QuantityVM = meal.Quantity.ToString();
            CaloriesVM = meal.Calories.ToString();
            MealPictureVM = meal.MealPicture;
            MealPictureSourceVM = meal.MealPictureSource;

            ComfirmEditMealCommand = new RelayCommand<EditMealView>((p) => true, (p) => EditMealCM(p));
            BrowseImageCommand = new RelayCommand<EditMealView>(p => true, p => _BrowseImage(p));
            //CancelEditMealCommand = new RelayCommand<EditMealView>(p => true, p => CancelCM(p));
        }
        void _LoadWindow()
        {
            var objectId = ObjectId.Parse(Meal.Id);
            var filter = Builders<Nutrition>.Filter.Eq("Id", objectId);
            var meal = _nutritionCollection.Find(filter).FirstOrDefault();

            if (meal != null)
            {
                Meal = meal;
                UserNameVM = meal.Username;
                MealTimeVM = meal.MealTime;
                MealNameVM = meal.MealName;
                DateTime parsedDay;
                if (DateTime.TryParseExact(meal.Day, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDay))
                {
                    DayVM = parsedDay;
                }
                UnitVM = meal.Unit;
                QuantityVM = meal.Quantity.ToString();
                CaloriesVM = meal.Calories.ToString();
                MealPictureVM = meal.MealPicture;
                MealPictureSourceVM = meal.MealPictureSource;
            }

        }
        public void EditMealCM(EditMealView parameter)
        {
            bool isEdit = false;
            var objectId = ObjectId.Parse(Meal.Id);
            var filter = Builders<Nutrition>.Filter.Eq("Id", objectId);
            var meal = _nutritionCollection.Find(filter).FirstOrDefault();

            if (MealTimeVM != meal.MealTime)
            {
                meal.MealTime = MealTimeVM;

                var updateDefinition = Builders<Nutrition>.Update.Set(u => u.MealTime, MealTimeVM);

                _nutritionCollection.UpdateOne(filter, updateDefinition);
                isEdit = true;
            }
            if (MealNameVM != meal.MealName)
            {
                meal.MealName = MealNameVM;

                var updateDefinition = Builders<Nutrition>.Update.Set(u => u.MealName, MealNameVM);

                _nutritionCollection.UpdateOne(filter, updateDefinition);
                isEdit = true;
            }
            string day = DayVM.HasValue ? DayVM.Value.ToString("dd/MM/yyyy") : null;
            if (day != meal.Day)
            {
                meal.Day = day;

                var updateDefinition = Builders<Nutrition>.Update.Set(u => u.Day, day);

                _nutritionCollection.UpdateOne(filter, updateDefinition);
                isEdit = true;
            }
            int quantity = int.Parse(QuantityVM);
            if (quantity != meal.Quantity)
            {
                meal.Quantity = quantity;

                var updateDefinition = Builders<Nutrition>.Update.Set(u => u.Quantity, quantity);

                _nutritionCollection.UpdateOne(filter, updateDefinition);
                isEdit = true;
            }
            int calories = int.Parse(CaloriesVM);
            if (calories != meal.Calories)
            {
                meal.Calories = calories;

                var updateDefinition = Builders<Nutrition>.Update.Set(u => u.Calories, calories);

                _nutritionCollection.UpdateOne(filter, updateDefinition);
                isEdit = true;
            }
            if (UnitVM != meal.Unit)
            {
                meal.Unit = UnitVM;

                var updateDefinition = Builders<Nutrition>.Update.Set(u => u.Unit, UnitVM);

                _nutritionCollection.UpdateOne(filter, updateDefinition);
                isEdit = true;
            }
            if (MealPictureVM != meal.MealPicture)
            {
                meal.MealPicture = MealPictureVM;

                var updateDefinition = Builders<Nutrition>.Update.Set(u => u.MealPicture, MealPictureVM);

                _nutritionCollection.UpdateOne(filter, updateDefinition);
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
        private void _BrowseImage(EditMealView parameter)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*",
                    Title = "Select an image file"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string imagePath = openFileDialog.FileName;
                    // Read the image into a byte array
                    byte[] imageData = File.ReadAllBytes(imagePath);

                    // Set the byte array to the ViewModel property
                    MealPictureVM = imageData;
                    // Load the image and set it to the Image control
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(imagePath);
                    bitmapImage.EndInit();

                    var objectId = ObjectId.Parse(Meal.Id);
                    var filter = Builders<Nutrition>.Filter.Eq("Id", objectId);
                    var meal = _nutritionCollection.Find(filter).FirstOrDefault();
                    meal.MealPicture = MealPictureVM;
                    var updateDefinition = Builders<Nutrition>.Update.Set(u => u.MealPicture, MealPictureVM);

                    _nutritionCollection.UpdateOne(filter, updateDefinition);
                    _LoadWindow();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
