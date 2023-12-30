using HealthCareApp.Models;
using HealthCareApp.Views;
using Microsoft.Win32;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace HealthCareApp.ViewModels
{
    internal class SignupViewModel : BaseViewModel
    {
        private readonly IMongoCollection<Account> _accountCollection;
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

        private DateTime? _birthday;
        public DateTime? BirthdayVM
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
        private string _password;
        public string PasswordVM
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(PasswordVM));
                }
            }
        }
        private string _repassword;
        public string RePassword
        {
            get { return _repassword; }
            set
            {
                if (_repassword != value)
                {
                    _repassword = value;
                    OnPropertyChanged(nameof(RePassword));
                }
            }
        }

        public ICommand SignUpCommand { get; set; }
        public ICommand PasswordchangeCM {  get; set; }
        public ICommand RePasswordchangeCM { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand BrowseImageCommand { get; set; }
        public SignupViewModel()
        {
            _accountCollection = GetMongoCollectionFromAccount();
            _userinfoCollection = GetMongoCollectionFromUserInfo();

            SignUpCommand = new RelayCommand<SignupView>((p) => true, (p) => SignUpCM(p));
            PasswordchangeCM = new RelayCommand<PasswordBox>((p) => true, (p) => { PasswordVM = p.Password; });
            RePasswordchangeCM = new RelayCommand<PasswordBox>((p) => true, (p) => { RePassword = p.Password; });
            CancelCommand = new RelayCommand<SignupView>((p) => true, (p) => {  p.Close(); });
            BrowseImageCommand = new RelayCommand<SignupView>(p => true, p => _BrowseImage(p));
        }

        public void SignUpCM(Window loginWindow)
        {
            if (ValidateSignUp())
            {
                //Create new account
                Account newAccount = new Account
                {
                    Username = UsernameVM,
                    Password = PasswordVM
                };

                _accountCollection.InsertOne(newAccount);

                //Create new user
                UserInformation newUser = new UserInformation
                {
                    Name = NameVM,
                    Gender = GenderVM,
                    PhoneNumber = PhoneNumberVM,
                    Birthday = BirthdayVM.HasValue ? BirthdayVM.Value.ToString("dd/MM/yyyy") : null, 
                    Address = AddressVM,
                    Email = EmailVM,
                    Username = UsernameVM,
                    Avatar = AvatarVM
                };


                _userinfoCollection.InsertOne(newUser);

                MessageBox.Show("Sign up successful!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);

                loginWindow.Close();
            }

        }
        private bool ValidateSignUp()
        {

            if (string.IsNullOrEmpty(UsernameVM) || string.IsNullOrEmpty(PasswordVM) || string.IsNullOrEmpty(RePassword))
            {
                MessageBox.Show("Please enter all required fields!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (PasswordVM != RePassword)
            {
                MessageBox.Show("Password and Confirm Password do not match!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
        private void _BrowseImage(SignupView parameter)
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
                    parameter.iconImage.Visibility = Visibility.Hidden;
                    parameter.loadedImage.Source = bitmapImage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private IMongoCollection<Account> GetMongoCollectionFromAccount()
        {
            // Set your MongoDB connection string and database name
            string connectionString = "mongodb+srv://HGB3009:HGB30092004@bao-database.xwrghva.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "HealthcareManagementDatabase"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Account>("Account");
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
