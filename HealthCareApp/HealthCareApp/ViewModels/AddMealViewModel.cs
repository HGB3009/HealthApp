using HealthCareApp.Models;
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
    internal class AddMealViewModel : BaseViewModel
    {
        private readonly IMongoCollection<Nutrition> _nutritionCollection;
        private string _username;
        public string UserNameVM
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(UserNameVM));
                }
            }
        }
        private string _mealtime;
        public string MealTimeVM
        {
            get { return _mealtime; }
            set
            {
                if (_mealtime != value)
                {
                    _mealtime = value;
                    OnPropertyChanged(nameof(MealTimeVM));
                }
            }
        }
        private string _mealname;
        public string MealNameVM
        {
            get { return _mealname; }
            set
            {
                if (_mealname != value)
                {
                    _mealname = value;
                    OnPropertyChanged(nameof(MealNameVM));
                }
            }
        }
        private DateTime? _day;
        public DateTime? DayVM
        {
            get { return _day; }
            set
            {
                if (_day != value)
                {
                    _day = value;
                    OnPropertyChanged(nameof(DayVM));
                }
            }
        }
        private string _quantity;
        public string QuantityVM
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(QuantityVM));
                }
            }
        }
        private string _unit;
        public string UnitVM
        {
            get { return _unit; }
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    OnPropertyChanged(nameof(UnitVM));
                }
            }
        }
        private string _calories;
        public string CaloriesVM
        {
            get { return _calories; }
            set
            {
                if (_calories != value)
                {
                    _calories = value;
                    OnPropertyChanged(nameof(CaloriesVM));
                }
            }
        }
        public ICommand CancelAddMealCommand { get; set; }
        public AddMealViewModel()
        {
            CancelAddMealCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
        }
    }
}
