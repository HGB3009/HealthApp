using HealthCareApp.Models;
using HealthCareApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using HealthCareApp.Components;

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
        public ICommand LogoutMainWD { get; set; }
        public ICommand AvatarImageSource { get; set; }

        public MainWindowViewModel()
        {
            MenuFieldCommand = new RelayCommand(MainMenu);
            FitnessFieldCommand = new RelayCommand(Fitness);
            SleepFieldCommand = new RelayCommand(Sleep);
            NutritionFieldCommand = new RelayCommand(Nutrition);
            DiagnosisFieldCommand = new RelayCommand(Diagnosis);
            RemindersFieldCommand = new RelayCommand(Reminders);
            SettingFieldCommand = new RelayCommand(Settings);
            CurrentView = new MenuView();
            LogoutMainWD = new RelayCommand<MainWindowView>((p) => true, (p) => _Logout(p));
        }
        private void MainMenu(object obj) => CurrentView = new MenuView();
        private void Fitness(object obj) => CurrentView = new FitnessView();
        private void Sleep(object obj) => CurrentView = new SleepView();
        private void Nutrition(object obj) => CurrentView = new NutritionView();
        private void Diagnosis(object obj) => CurrentView = new DiagnosisView();
        private void Reminders(object obj) => CurrentView = new RemindersView();

        private void Settings(object obj) => CurrentView = new SettingsView();
        void _Logout(MainWindowView mainWindow)
        {

            //var window = Window.GetWindow(mainWindow);
            //if (window != null)
            //{
            //    window.Close();
            //}
        }
    }
}
