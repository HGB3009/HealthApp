using HealthCareApp.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthCareApp.ViewModels
{
    class ForgotPasswordViewModel:BaseViewModel
    {
        private readonly IMongoCollection<Account> _accountCollection;
        public static bool IsValid { get; set; }
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
        public ForgotPasswordViewModel() 
        {

        }    
        private string GenerateRandomVerificationCode()
        {
            Random random = new Random();
            int code = random.Next(100000, 999999);
            return code.ToString();
        }
        private bool ValidateResetPassword()
        {

            return true;
        }
    }
}
