using HealthCareApp.Models;
using HealthCareApp.Views;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MongoDB.Bson.Serialization.Conventions;

namespace HealthCareApp.ViewModels
{
    class ForgotPasswordViewModel : BaseViewModel
    {
        private readonly IMongoCollection<Account> _accountCollection;
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

        private string _verification;
        public string VerificationVM
        {
            get { return _verification; }
            set
            {
                if (_verification != value)
                {
                    _verification = value;
                    OnPropertyChanged(nameof(VerificationVM));
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
        private bool _isValidCode;
        public bool isValidCode
        {
            get { return _isValidCode; }
            set
            {
                if (_isValidCode != value)
                {
                    _isValidCode = value;
                    OnPropertyChanged(nameof(isValidCode));
                }
            }
        }

        private bool _isValidEmail;
        public bool isValidEmail
        {
            get { return _isValidEmail; }
            set
            {
                if (_isValidEmail != value)
                {
                    _isValidEmail = value;
                    OnPropertyChanged(nameof(isValidEmail));
                }
            }
        }

        public string verificationCode { get; set; }
        public ICommand ChangePasswordCM { get; set; }
        public ICommand SendingVerificationCM { get; set; }
        public ICommand CheckVerificationPasswordCM { get; set; }
        public ICommand NewPasswordchangeCM { get; set; }
        public ICommand ReNewPasswordchangeCM { get; set; }
        public ForgotPasswordViewModel()
        {
            _accountCollection = GetMongoCollectionFromAccount();
            _userinfoCollection = GetMongoCollectionFromUserInfo();

            ChangePasswordCM = new RelayCommand<ForgotPasswordView>((parameter) => true, (parameter) => ChangePassword(parameter));
            SendingVerificationCM = new RelayCommand<ForgotPasswordView>((parameter) => true, (parameter) => SendingVerification(parameter));
            CheckVerificationPasswordCM = new RelayCommand<ForgotPasswordView>((parameter) => true, (parameter) => CheckVerificationPassword(parameter));
            NewPasswordchangeCM = new RelayCommand<PasswordBox>((p) => true, (p) => { PasswordVM = p.Password; });
            ReNewPasswordchangeCM = new RelayCommand<PasswordBox>((p) => true, (p) => { RePassword = p.Password; });
        }
        public void ChangePassword(ForgotPasswordView parameter)
        {
            if (!string.IsNullOrEmpty(PasswordVM))
            {
                if(!string.IsNullOrEmpty(RePassword))
                {
                    if (PasswordVM == RePassword)
                    {
                        var account = GetAccount(UsernameVM);
                        var filter = Builders<Account>.Filter.Eq(u => u.Username, UsernameVM);

                        if (account != null)
                        {
                            account.Password = PasswordVM;

                            var updateDefinition = Builders<Account>.Update.Set(u => u.Password, PasswordVM);

                            _accountCollection.UpdateOne(filter, updateDefinition);

                            MessageBox.Show("Password changed successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                            parameter.Close();
                        }
                        else
                        {
                            MessageBox.Show("Username not found!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                            parameter.Email.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Please enter matching new password and re-enter new password!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                        parameter.NewPassword.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Please enter re-enter new password!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                    parameter.ReNewPassword.Focus();
                }
            }
            else
            {
                MessageBox.Show("Please enter new password!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.NewPassword.Focus();
            }
        }
        public void SendingVerification(ForgotPasswordView p)
        {
            if (ValidateEmail(p))
            {
                try
                {
                    verificationCode = GenerateRandomVerificationCode();
                    string smtpHost = "smtp.gmail.com";
                    int smtpPort = 587;
                    string smtpUsername = "healthcareappit008@gmail.com";
                    string smtpPassword = "stuc kjtq ukmb kedx";
                    SmtpClient client = new SmtpClient(smtpHost, smtpPort)
                    {
                        Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                        EnableSsl = true
                    };


                    MailMessage mail = new MailMessage
                    {
                        From = new MailAddress("healthcareappit008@gmail.com"),
                        Subject = "Verification Code",
                        Body = $"Your verification code is: {verificationCode}",
                        IsBodyHtml = false
                    };

                    mail.To.Add(EmailVM);
                    client.Send(mail);
                    isValidEmail = true;
                    ReloadWindow(p);
                    MessageBox.Show("Verification code sent to registration email!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error sending verification code: {ex.Message}", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        public void CheckVerificationPassword(ForgotPasswordView p)
        {
            if (ValidateResetPassword(p))
            {
                isValidCode = true;
                ReloadWindow(p);
            }
        }
        private string GenerateRandomVerificationCode()
        {
            Random random = new Random();
            int code = random.Next(100000, 999999);
            return code.ToString();
        }
        private bool ValidateResetPassword(ForgotPasswordView p)
        {
            if (!string.IsNullOrEmpty(VerificationVM))
            {
                if (VerificationVM != verificationCode)
                {
                    MessageBox.Show("Your verification code does not match!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                    p.Vertification.Focus();
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                MessageBox.Show("Please enter the verification code!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                p.Vertification.Focus();
                return false;
            }
        }
        private bool ValidateEmail(ForgotPasswordView p)
        {
            if (!string.IsNullOrEmpty(EmailVM))
            {
                var user = GetUserInfo(EmailVM);

                if (user == null)
                {
                    MessageBox.Show("The email you entered is not registered for an account!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                    p.Email.Focus();
                    return false;
                }

                UsernameVM = user.Username;
                return true;
            }
            else
            {
                MessageBox.Show("Please enter the email!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                p.Email.Focus();
                return false;
            }
        }
        private Account GetAccount(string Username)
        {
            var filter = Builders<Account>.Filter.Eq(u => u.Username, Username);
            return _accountCollection.Find(filter).FirstOrDefault();
        }

        private UserInformation GetUserInfo(string Email)
        {
            var filter = Builders<UserInformation>.Filter.Eq(u => u.Email, Email);
            return _userinfoCollection.Find(filter).FirstOrDefault();
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
        public void ReloadWindow(ForgotPasswordView currentWindow)
        {
            ForgotPasswordView newWindow = new ForgotPasswordView();
            newWindow.DataContext = currentWindow.DataContext;
            currentWindow.Close();
            newWindow.Show();
        }
    }
}