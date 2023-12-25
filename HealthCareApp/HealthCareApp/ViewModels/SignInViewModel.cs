using HealthCareApp.Models;
using HealthCareApp.Views;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HealthCareApp.ViewModels
{
    internal class SignInViewModel:BaseViewModel
    {
        private readonly IMongoCollection<Account> _accountCollection;
        public static bool IsLogin { get; set; }
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }
        public ICommand SignInCommand { get; set; }
        public ICommand OpenSignUpWindowCommand { get; set; }
        public ICommand OpenChangePasswordCommand { get; set; }
        public ICommand PasswordchangeCM {  get; set; }
        public SignInViewModel() 
        {
            _accountCollection = GetMongoCollection();
            SignInCommand = new RelayCommand<SignInView>((parameter) => true, (parameter) => SignInCM(parameter));
            OpenSignUpWindowCommand = new RelayCommand<SignInView>((parameter) => true, (parameter) => OpenSignUpWindow(parameter));
            OpenChangePasswordCommand = new RelayCommand<SignInView>((parameter) => true, (parameter) => OpenForgotPasswordWindow(parameter));
            PasswordchangeCM = new RelayCommand<PasswordBox>((p) => true, (p) => { Password = p.Password; });
        }
        public void SignInCM(Window loginWindow)
        {

            if (!string.IsNullOrEmpty(Username))
            {
                if (!string.IsNullOrEmpty(Password))
                {
                    var user = GetUser(Username);

                    if (user == null)
                    {
                        MessageBox.Show("Wrong username or password!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (Password == user.Password)
                    {
                        MainWindowView mainWindow = new MainWindowView();
                        mainWindow.Show();
                        IsLogin = true;
                        loginWindow.Close();
                    }
                    else
                    {
                        MessageBox.Show("Wrong username or password!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter the password!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            } 
            else
            {
                MessageBox.Show("Please enter the username!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private IMongoCollection<Account> GetMongoCollection()
        {
            // Set your MongoDB connection string and database name
            string connectionString = "mongodb+srv://HGB3009:HGB30092004@bao-database.xwrghva.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "HealthcareManagementDatabase"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Account>("Account");
        }

        private Account GetUser(string Username)
        {
            var filter = Builders<Account>.Filter.Eq(u => u.Username, Username);
            return _accountCollection.Find(filter).FirstOrDefault();
        }

        public void OpenForgotPasswordWindow(Window loginWindow)
        {
            ForgotPasswordView forgotPasswordWindow = new ForgotPasswordView();

            //forgotPasswordWindow.Username.Text = null;
            loginWindow.Opacity = 0.5;
            loginWindow.WindowStyle = WindowStyle.None;
            forgotPasswordWindow.ShowDialog();
            loginWindow.WindowStyle = WindowStyle.SingleBorderWindow;
            loginWindow.Opacity = 1;
            loginWindow.Show();
        }
        public void OpenSignUpWindow(Window loginWindow)
        {
            SignupView signupWindow = new SignupView();

            //forgotPasswordWindow.Username.Text = null;
            loginWindow.Opacity = 0.5;
            loginWindow.WindowStyle = WindowStyle.None;
            signupWindow.ShowDialog();
            loginWindow.WindowStyle = WindowStyle.SingleBorderWindow;
            loginWindow.Opacity = 1;
            loginWindow.Show();
            
        }
    }
}
