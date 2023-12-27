using HealthCareApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HealthCareApp.ViewModels
{
    public class LeftSideBarViewModel:BaseViewModel
    {
        public ICommand MainMenuFieldCommand { get; set; }
        public ICommand NutritionFieldCommand { get; set; }
        public ICommand FitnessFieldCommand { get; set; }
        public ICommand SleepFieldCommand { get; set; }
        public ICommand DiagnosisFieldCommand { get; set; }
        public ICommand RemindersFieldCommand { get; set; }
        public ICommand SettingFieldCommand { get; set; }

        public LeftSideBarViewModel()
        {
            MainMenuFieldCommand=new RelayCommand<Window>((p) => { return true; },(p) => { MainWindowView window = new MainWindowView();window.ShowDialog(); });
            FitnessFieldCommand = new RelayCommand<Window>((p) => { return true; },(p) => { FitnessView window = new FitnessView(); window.ShowDialog(); });
            SleepFieldCommand = new RelayCommand<Window>((p) => { return true; },(p) => {  });
            NutritionFieldCommand = new RelayCommand<Window>((p) => { return true; },(p) => { NutrientsView window = new NutrientsView(); window.ShowDialog(); });
            DiagnosisFieldCommand = new RelayCommand<Window>((p) => { return true; },(p) => { SymtompCheckerView window = new SymtompCheckerView();window.ShowDialog(); });
            RemindersFieldCommand = new RelayCommand<Window>((p) => { return true; },(p) => { });
            SettingFieldCommand = new RelayCommand<Window>((p) => { return true; },(p) => { SettingView window = new SettingView(); window.ShowDialog(); });

        }
    }
}
