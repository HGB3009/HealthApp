using HealthCareApp.Models;
using HealthCareApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace HealthCareApp.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        private UserInformation _currentUser;
        public UserInformation CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; OnPropertyChanged(); }
        }
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        public ICommand MenuFieldCommand { get; set; }
        public ICommand NutritionFieldCommand { get; set; }
        public ICommand FitnessFieldCommand { get; set; }
        public ICommand SleepFieldCommand { get; set; }
        public ICommand DiagnosisFieldCommand { get; set; }
        public ICommand RemindersFieldCommand { get; set; }
        public ICommand SettingFieldCommand { get; set; }

        public MainWindowViewModel()
        {
            MenuFieldCommand = new RelayCommand(MainMenu);
            FitnessFieldCommand = new RelayCommand(Fitness);
            SleepFieldCommand = new RelayCommand(Sleep);
            RemindersFieldCommand = new RelayCommand(Reminders);
            SettingFieldCommand = new RelayCommand(Settings);
            CurrentView = new MenuView();
        }
        private void MainMenu(object obj) => CurrentView = new MenuView();
        private void Fitness(object obj) => CurrentView = new FitnessView();
        private void Sleep(object obj) => CurrentView = new SleepView();
        private void Reminders(object obj) => CurrentView = new RemindersView();
        private void Settings(object obj) => CurrentView = new SettingsView();
    }
}
