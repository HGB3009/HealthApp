using MaterialDesignColors;
using OxyPlot;
using OxyPlot.Series;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using FontWeights = OxyPlot.FontWeights;

namespace HealthCareApp.ViewModels
{
    public class RemindersViewModel : INotifyPropertyChanged
    {
        private PlotModel _waterIntakeModel;
        private double _consumedWater;
        private double _targetWater;
        private string _breakfastTime;
        private string _lunchTime;
        private string _snackTime;
        private string _dinnerTime;

        public RemindersViewModel()
        {
            _targetWater = 2000;
            _consumedWater = 500;
            CreateWaterIntakeModel();
            EditCommand = new RelayCommand(o => ExecuteEdit());
            SaveCommand = new RelayCommand(o => ExecuteSave(), o => CanExecuteSave());
        }

        public PlotModel WaterIntakeModel
        {
            get => _waterIntakeModel;
            set
            {
                _waterIntakeModel = value;
                OnPropertyChanged();
            }
        }
        public double TargetWater
        {
            get => _targetWater;
            set
            {
                if (_targetWater != value)
                {
                    _targetWater = value;
                    UpdateWaterIntakeModel();
                    OnPropertyChanged();
                }
            }
        }

        public double ConsumedWater
        {
            get => _consumedWater;
            set
            {
                _consumedWater = value;
                UpdateWaterIntakeModel();
                OnPropertyChanged();
            }
        }

        private void CreateWaterIntakeModel()
        {
            _waterIntakeModel = new PlotModel { Title = "Water Intake" };
            UpdateWaterIntakeModel();
        }

        private void UpdateWaterIntakeModel()
        {
            _waterIntakeModel.Series.Clear();
            _waterIntakeModel.Title = null;

            var consumedSlice = new PieSlice("Drinked", _consumedWater)
            {
                IsExploded = true,
                Fill = OxyColor.Parse("#0E87CC") 
            };

            var remainingSlice = new PieSlice("Remaining", _targetWater - _consumedWater)
            {
                Fill = OxyColor.Parse("#BDBDBD")
            };

            var pieSeries = new PieSeries
            {
                Slices = new List<PieSlice> { consumedSlice, remainingSlice },
                StrokeThickness = 2.5,
                InsideLabelPosition = 0.55,
                InsideLabelColor = OxyColor.Parse("#CC0E86"),
                AngleSpan = 360,
                StartAngle = 0,
                FontSize = 18,
                FontWeight = FontWeights.Bold
            };

            _waterIntakeModel.Series.Add(pieSeries);

            _waterIntakeModel.InvalidatePlot(true);
            OnPropertyChanged(nameof(WaterIntakeModel));
        }

        public ICommand EditCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        private void ExecuteEdit()
        {
        }

        private void ExecuteSave()
        {
        }

        private bool CanExecuteSave()
        {
            return true;
        }
        public string BreakfastTime
        {
            get => _breakfastTime;
            set
            {
                if (_breakfastTime != value)
                {
                    _breakfastTime = value;
                    OnPropertyChanged();
                }
            }
        }

        // Property for Lunch Time
        public string LunchTime
        {
            get => _lunchTime;
            set
            {
                if (_lunchTime != value)
                {
                    _lunchTime = value;
                    OnPropertyChanged();
                }
            }
        }

        // Property for Snack Time
        public string SnackTime
        {
            get => _snackTime;
            set
            {
                if (_snackTime != value)
                {
                    _snackTime = value;
                    OnPropertyChanged();
                }
            }
        }

        // Property for Dinner Time
        public string DinnerTime
        {
            get => _dinnerTime;
            set
            {
                if (_dinnerTime != value)
                {
                    _dinnerTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}