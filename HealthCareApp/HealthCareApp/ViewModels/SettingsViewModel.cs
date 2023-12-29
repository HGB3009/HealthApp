using HealthCareApp.Models;
using HealthCareApp.Views;
using Microsoft.Win32;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

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
        private BitmapImage _avatarimagesource;
        public BitmapImage AvatarImageSourceVM
        {
            get => _avatarimagesource;
            set
            {
                _avatarimagesource = value;
                OnPropertyChanged(nameof(AvatarImageSourceVM));
            }
        }
        public ICommand UpdateInfoCM { get; set; }
        public ICommand ChangePasswordCM { get; set; }
        public ICommand BrowseImageCommand { get; set; }
        public ICommand LoadWindowCommand { get; set; }
        public event EventHandler MainWindowReloadRequested;
        public SettingsViewModel() 
        {
            _userinfoCollection = GetMongoCollectionFromUserInfo();

            LoadWindowCommand = new RelayCommand<SettingsView>((p) => true, (p) => _LoadWindow(p));
            UpdateInfoCM = new RelayCommand<SettingsView>((parameter) => true, (parameter) => UpdateInfo(parameter));
            ChangePasswordCM = new RelayCommand<SettingsView>((parameter) => true, (parameter) => ChangePassword(parameter));
            BrowseImageCommand = new RelayCommand<SettingsView>(p => true, p => _BrowseImage(p));
        }
        public void UpdateInfo(SettingsView parameter)
        {
            UpdateInformationView UpdateWindow = new UpdateInformationView();

            //forgotPasswordWindow.Username.Text = null;
            parameter.Opacity = 0.5;
            UpdateWindow.ShowDialog();
            parameter.Opacity = 1;
            _LoadWindow(parameter);
            parameter.Focus();
        }
        public void ChangePassword(SettingsView parameter) 
        {
            
        }
        void _LoadWindow(SettingsView p)
        {
            string username = Const.Instance.Username;

            var filter = Builders<UserInformation>.Filter.Eq(x => x.Username, username);
            var User = _userinfoCollection.Find(filter).FirstOrDefault();

            if (User != null)
            {
                NameVM = ": " + User.Name;
                GenderVM = ": " + User.Gender;
                PhoneNumberVM = ": " + User.PhoneNumber;
                BirthdayVM = ": " + User.Birthday;
                AddressVM = ": " + User.Address;
                EmailVM = ": " + User.Email;
                AvatarImageSourceVM = User.AvatarImageSource;
            }
            
        }

        private void OnMainWindowReloadRequested()
        {
            MainWindowReloadRequested?.Invoke(this, EventArgs.Empty);
        }
        private void _BrowseImage(SettingsView parameter)
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
                    AvatarVM = imageData;
                    // Load the image and set it to the Image control
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(imagePath);
                    bitmapImage.EndInit();

                    string username = Const.Instance.Username;
                    var filter = Builders<UserInformation>.Filter.Eq(x => x.Username, username);
                    var User = _userinfoCollection.Find(filter).FirstOrDefault();
                    User.Avatar = AvatarVM;
                    var updateDefinition = Builders<UserInformation>.Update.Set(u => u.Avatar, AvatarVM);

                    _userinfoCollection.UpdateOne(filter, updateDefinition);
                    OnMainWindowReloadRequested();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
