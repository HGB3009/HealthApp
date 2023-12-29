using HealthCareApp.Models;
using HealthCareApp.Views;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthCareApp.ViewModels
{

    public class SettingsViewModel : BaseViewModel
    {
        private readonly IMongoCollection<UserInformation> _userinfoCollection;
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

        private string _gender;
        public string GenderVM
        {
            get { return _gender; }
            set
            {
                if (_gender != value)
                {
                    _gender = value;
                    OnPropertyChanged(nameof(GenderVM));
                }
            }
        }

        private string _phoneNumber;
        public string PhoneNumberVM
        {
            get { return _phoneNumber; }
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumberVM));
                }
            }
        }

        private string _birthday;
        public string BirthdayVM
        {
            get { return _birthday; }
            set
            {
                if (_birthday != value)
                {
                    _birthday = value;
                    OnPropertyChanged(nameof(BirthdayVM));
                }
            }
        }

        private string _address;
        public string AddressVM
        {
            get { return _address; }
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged(nameof(AddressVM));
                }
            }
        }

        private string _email;
        public string EmailVM
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(EmailVM));
                }
            }
        }
        private byte[] _avatar;
        public byte[] AvatarVM
        {
            get { return _avatar; }
            set
            {
                if (_avatar != value)
                {
                    _avatar = value;
                    OnPropertyChanged(nameof(AvatarVM));
                }
            }
        }
        public ICommand UpdateInfoCM { get; set; }
        public ICommand ChangePasswordCM { get; set; }
        public ICommand BrowseImageCommand { get; set; }
        public SettingsViewModel() 
        {
            _userinfoCollection = GetMongoCollectionFromUserInfo();

            UpdateInfoCM = new RelayCommand<SettingsView>((parameter) => true, (parameter) => UpdateInfo(parameter));
            ChangePasswordCM = new RelayCommand<SettingsView>((parameter) => true, (parameter) => ChangePassword(parameter));
        }
        public void UpdateInfo(SettingsView parameter)
        {

        }
        public void ChangePassword(SettingsView parameter) 
        {
            
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
