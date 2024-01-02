using HealthCareApp.Models;
using HealthCareApp.Views;
using Microsoft.Win32;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace HealthCareApp.ViewModels
{
    internal class AddMealViewModel : BaseViewModel
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
        public ICommand CancelAddMealCommand { get; set; }
        public ICommand BrowseImageCommand { get; set; }
        public ICommand AddMealCommand { get; set; }
        public AddMealViewModel()
        {
            _nutritionCollection = GetMongoCollectionFromNutrition();
            AddMealCommand = new RelayCommand<AddMealView>((p) => true, (p) => AddMealCM(p));
            BrowseImageCommand = new RelayCommand<AddMealView>(p => true, p => _BrowseImage(p));
            CancelAddMealCommand = new RelayCommand<AddMealView>((p) => { return true; }, (p) => CancelCM(p));
        }
        public void CancelCM(AddMealView p)
        {
            MealNameVM = null;
            MealTimeVM = null;
            DayVM = null;
            QuantityVM = null;
            UnitVM = null;
            CaloriesVM = null;
            MealTimeVM = null;
            p.loadedImage.Source = null;
            p.Close();
        }
        public void AddMealCM(AddMealView parameter)
        {
            if (ValidateInput(parameter))
            {
                //Create new meal
                Nutrition newNutrition = new Nutrition
                {
                    Username = Const.Instance.Username,
                    MealName = MealNameVM,
                    MealTime = MealTimeVM,
                    Day = DayVM.HasValue ? DayVM.Value.ToString("dd/MM/yyyy") : null,
                    Quantity = int.Parse(QuantityVM),
                    Unit = UnitVM,
                    Calories = float.Parse(CaloriesVM),
                    MealPicture = MealPictureVM,
                };
                _nutritionCollection.InsertOne(newNutrition);

                MessageBox.Show("Add meal successful!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                MealNameVM = null;
                MealTimeVM = null;
                DayVM = null;
                QuantityVM = null;
                UnitVM = null;
                CaloriesVM = null;
                MealTimeVM = null;
                parameter.loadedImage.Source = null;
                parameter.Close();
            }
        }
        private bool ValidateInput(AddMealView parameter)
        {
            if (string.IsNullOrEmpty(MealNameVM))
            {
                MessageBox.Show("Please enter the meal name!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.MealName.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(MealTimeVM))
            {
                MessageBox.Show("Please select the meal time!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.MealTime.Focus();
                return false;
            }
            string day = DayVM.HasValue ? DayVM.Value.ToString("dd/MM/yyyy") : null;
            if (string.IsNullOrEmpty(day))
            {
                MessageBox.Show("Please select the day!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.Day.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(QuantityVM))
            {
                MessageBox.Show("Please enter the quantity!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.Quantity.Focus();
                return false;
            }
            if (!int.TryParse(QuantityVM, out _))
            {
                MessageBox.Show("Please enter the quantity in numeric format!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.Quantity.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(UnitVM))
            {
                MessageBox.Show("Please enter the unit!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.Unit.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(CaloriesVM))
            {
                MessageBox.Show("Please enter the calories!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.Calories.Focus();
                return false;
            }
            if (!double.TryParse(CaloriesVM, out _))
            {
                MessageBox.Show("Please enter the calories in numeric format!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.Calories.Focus();
                return false;
            }
            return true;
        }
        private void _BrowseImage(AddMealView parameter)
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
                    parameter.mealIcon.Visibility = Visibility.Hidden;
                    parameter.loadedImage.Source = bitmapImage;
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
