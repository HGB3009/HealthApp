using HealthCareApp.Models;
using HealthCareApp.Views;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HealthCareApp.ViewModels
{
    public class UpdateBodyViewModel : BaseViewModel
    {
        private readonly IMongoCollection<UserInformation> _userinfoCollection;
        private string _weight;
        public string WeightVM
        {
            get { return _weight; }
            set
            {
                if (_weight != value)
                {
                    _weight = value;
                    OnPropertyChanged(nameof(WeightVM));
                }
            }
        }
        private string _height;
        public string HeightVM
        {
            get { return _height; }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged(nameof(HeightVM));
                }
            }
        }
        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public UpdateBodyViewModel()
        {
            _userinfoCollection = GetMongoCollectionFromUserInfo();
            ConfirmCommand = new RelayCommand<UpdateBodyView>((parameter) => true, (parameter) => ConfirmCM(parameter));
            CancelCommand = new RelayCommand<UpdateBodyView>(p => true, p => CancelCM(p));
        }
        public void ConfirmCM(UpdateBodyView view)
        {
            if (ValidateInput(view))
            {
                string username = Const.Instance.Username;
                var filter = Builders<UserInformation>.Filter.Eq(x => x.Username, username);
                var User = _userinfoCollection.Find(filter).FirstOrDefault();
                User.Weight = double.Parse(WeightVM);
                User.Height = double.Parse(HeightVM);
                double weight = double.Parse(WeightVM);
                double height = double.Parse(HeightVM);
                var updateDefinition = Builders<UserInformation>.Update.Set(u => u.Weight, weight);

                _userinfoCollection.UpdateOne(filter, updateDefinition);
                var updateDefinition1 = Builders<UserInformation>.Update.Set(u => u.Height, height);

                _userinfoCollection.UpdateOne(filter, updateDefinition1);

                MessageBox.Show("Information changed successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);

                WeightVM = null;
                HeightVM = null;
                view.Close();
            }

        }
        public void CancelCM(UpdateBodyView view)
        {
            WeightVM = null;
            HeightVM = null;
            view.Close();
        }
        private bool ValidateInput(UpdateBodyView parameter)
        {
            if (string.IsNullOrEmpty(WeightVM))
            {
                MessageBox.Show("Please enter the weight!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.Weight.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(HeightVM))
            {
                MessageBox.Show("Please enter the height!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.Height.Focus();
                return false;
            }
            if (!double.TryParse(WeightVM, out _))
            {
                MessageBox.Show("Please enter the weight in numeric format!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.Weight.Focus();
                return false;
            }
            if (!double.TryParse(HeightVM, out _))
            {
                MessageBox.Show("Please enter the height in numeric format!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.Weight.Focus();
                return false;
            }
            return true;
        }
        private IMongoCollection<UserInformation> GetMongoCollectionFromUserInfo()
        {
            // Set your MongoDB connection string and database name
            string connectionString = "mongodb+srv://HGB3009:HGB30092004@bao-database.xwrghva.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "HealthcareManagementDatabase"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<UserInformation>("UserInformation");
        }
    }
}
