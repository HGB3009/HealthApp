using HealthCareApp.Components;
using HealthCareApp.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace HealthCareApp.ViewModels
{
    public class UserInformationViewModel : BaseViewModel
    {
        private readonly IMongoCollection<UserInformation> _userinfoCollection;
        private string _username;
        public string UsernameVM
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(UsernameVM));
                }
            }
        }
        private string _name;
        public string NameVM
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(NameVM));
                }
            }
        }
        private string _age;
        public string AgeVM
        {
            get { return _age; }
            set
            {
                if (_age != value)
                {
                    _age = value;
                    OnPropertyChanged(nameof(AgeVM));
                }
            }
        }
        private BitmapImage _avatarimagesource;
        public BitmapImage AvatarImageSource
        {
            get => _avatarimagesource;
            set
            {
                _avatarimagesource = value;
                OnPropertyChanged(nameof(AvatarImageSource));
            }
        }
        public ICommand LoadWindowCommand { get; set; }
        public UserInformationViewModel()
        {
            _userinfoCollection = GetMongoCollectionFromUserInfo();
            LoadWindowCommand = new RelayCommand<NavigationBarView>((p) => true, (p) => _LoadWindow());
        }
        void _LoadWindow()
        {
            string username = Const.Instance.Username;
            UsernameVM = "Hello " + username + "!";

            var filter = Builders<UserInformation>.Filter.Eq(x => x.Username, username);
            var User = _userinfoCollection.Find(filter).FirstOrDefault();

            if (User != null)
            {
                NameVM = User.Name;
                AvatarImageSource = User.AvatarImageSource;
                string birthdayString = User.Birthday;
                if (DateTime.TryParseExact(birthdayString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime birthday))
                {
                    int age = DateTime.Now.Year - birthday.Year;

                    if (DateTime.Now < birthday.AddYears(age))
                    {
                        age--;
                    }

                    AgeVM = "Age: " + age.ToString();
                }
            }
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
