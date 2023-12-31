using HealthCareApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthCareApp.ViewModels
{
    public class SleepViewModel : BaseViewModel
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        public ICommand ChartShowCommand { get; set; }
        public ICommand HistoryCommand { get; set; }
        public ICommand SleepTrackerCommand { get; set; }
        public ICommand LoadWindowCommand { get; set; }
        public SleepViewModel()
        {
            ChartShowCommand = new RelayCommand(SleepChart);
            HistoryCommand = new RelayCommand(SleepHistory);
            CurrentView = new SleepChartView();
        }
        private void SleepChart(object obj) => CurrentView = new SleepChartView();
        private void SleepHistory(object obj) => CurrentView = new SleepHistoryView();
    }
}
