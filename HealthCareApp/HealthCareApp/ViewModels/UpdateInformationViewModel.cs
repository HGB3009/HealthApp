using HealthCareApp.Models;
using HealthCareApp.Views;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HealthCareApp.ViewModels
{
    public class UpdateInformationViewModel : BaseViewModel
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
        public ICommand UpdateInformationCM { get; set; }
        public UpdateInformationViewModel()
        {
            _userinfoCollection = GetMongoCollectionFromUserInfo();
            string username = Const.Instance.Username;
            var filter = Builders<UserInformation>.Filter.Eq(x => x.Username, username);
            var User = _userinfoCollection.Find(filter).FirstOrDefault();

            if (User != null)
            {
                NameVM = User.Name;
                GenderVM = User.Gender;
                PhoneNumberVM = User.PhoneNumber;
                DateTime parsedBirthday;
                if (DateTime.TryParseExact(User.Birthday, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out parsedBirthday))
                {
                    BirthdayVM = parsedBirthday;
                }
                AddressVM = User.Address;
                EmailVM = User.Email;
            }



            UpdateInformationCM = new RelayCommand<UpdateInformationView>((p) => true, (p) => UpdateCM(p));
        }
        public void UpdateCM(UpdateInformationView parameter)
        {
            bool isUpdate = false;
            string username = Const.Instance.Username;
            var filter = Builders<UserInformation>.Filter.Eq(x => x.Username, username);
            var User = _userinfoCollection.Find(filter).FirstOrDefault();
            if (NameVM != User.Name)
            {
                User.Name = NameVM;

                var updateDefinition = Builders<UserInformation>.Update.Set(u => u.Name, NameVM);

                _userinfoCollection.UpdateOne(filter, updateDefinition);
                isUpdate = true;
            }
            if (GenderVM != User.Gender)
            {
                User.Gender = GenderVM;

                var updateDefinition = Builders<UserInformation>.Update.Set(u => u.Gender, GenderVM);

                _userinfoCollection.UpdateOne(filter, updateDefinition);
                isUpdate = true;
            }
            if (PhoneNumberVM != User.PhoneNumber)
            {
                User.PhoneNumber = PhoneNumberVM;

                var updateDefinition = Builders<UserInformation>.Update.Set(u => u.PhoneNumber, PhoneNumberVM);

                _userinfoCollection.UpdateOne(filter, updateDefinition);
                isUpdate = true;
            }
            string birthday = BirthdayVM.HasValue ? BirthdayVM.Value.ToString("dd/MM/yyyy") : null;
            if (birthday != User.Birthday)
            {
                User.Birthday = birthday;

                var updateDefinition = Builders<UserInformation>.Update.Set(u => u.Birthday, birthday);

                _userinfoCollection.UpdateOne(filter, updateDefinition);
                isUpdate = true;
            }
            if (AddressVM != User.Address)
            {
                User.Address = AddressVM;

                var updateDefinition = Builders<UserInformation>.Update.Set(u => u.Address, AddressVM);

                _userinfoCollection.UpdateOne(filter, updateDefinition);
                isUpdate = true;
            }
            if (EmailVM != User.Email)
            {
                User.Email = EmailVM;

                var updateDefinition = Builders<UserInformation>.Update.Set(u => u.Email, EmailVM);

                _userinfoCollection.UpdateOne(filter, updateDefinition);
                isUpdate = true;
            }

            if (isUpdate)
            {
                MessageBox.Show("Information changed successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Nothing changed!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
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
