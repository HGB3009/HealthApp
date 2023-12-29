using HealthCareApp.Models;
using HealthCareApp.Views;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HealthCareApp.ViewModels
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        private readonly IMongoCollection<Account> _accountCollection;
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
        private string _oldpassword;
        public string OldPasswordVM
        {
            get { return _oldpassword; }
            set
            {
                if (_oldpassword != value)
                {
                    _oldpassword = value;
                    OnPropertyChanged(nameof(OldPasswordVM));
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
        public string RePasswordVM
        {
            get { return _repassword; }
            set
            {
                if (_repassword != value)
                {
                    _repassword = value;
                    OnPropertyChanged(nameof(RePasswordVM));
                }
            }
        }
        public ICommand OldPasswordChangeCM { get; set; }
        public ICommand NewPasswordchangeCM { get; set; }
        public ICommand ReNewPasswordchangeCM { get; set; }
        public ICommand ChangePasswordCM { get; set; }
        public ChangePasswordViewModel()
        {
            _accountCollection = GetMongoCollectionFromAccount();

            ChangePasswordCM = new RelayCommand<ChangePasswordView>((parameter) => true, (parameter) => ChangePassword(parameter));
            OldPasswordChangeCM = new RelayCommand<PasswordBox>((p) => true, (p) => { OldPasswordVM = p.Password; });
            NewPasswordchangeCM = new RelayCommand<PasswordBox>((p) => true, (p) => { PasswordVM = p.Password; });
            ReNewPasswordchangeCM = new RelayCommand<PasswordBox>((p) => true, (p) => { RePasswordVM = p.Password; });
        }
        public void ChangePassword(ChangePasswordView parameter)
        {
            if (!string.IsNullOrEmpty(OldPasswordVM))
            {
                if (!string.IsNullOrEmpty(PasswordVM))
                {
                    if (!string.IsNullOrEmpty(RePasswordVM))
                    {
                        string username = Const.Instance.Username;
                        var filter = Builders<Account>.Filter.Eq(x => x.Username, username);
                        var User = _accountCollection.Find(filter).FirstOrDefault();
                        if (OldPasswordVM == User.Password)
                        {
                            if (PasswordVM == RePasswordVM)
                            {
                                User.Password = PasswordVM;
                                var updateDefinition = Builders<Account>.Update.Set(u => u.Password, PasswordVM);
                                _accountCollection.UpdateOne(filter, updateDefinition);
                                MessageBox.Show("Password changed successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                                parameter.Close();
                            }
                            else
                            {
                                MessageBox.Show("Please enter matching new password and re-enter new password!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                                parameter.NewPassword.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please enter the correct old password.!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                            parameter.OldPassword.Focus();
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
            else
            {
                MessageBox.Show("Please enter old password!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.OldPassword.Focus();
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
    }
}
