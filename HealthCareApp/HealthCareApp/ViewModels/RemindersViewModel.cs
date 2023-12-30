using MaterialDesignColors;
using OxyPlot;
using OxyPlot.Series;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace HealthCareApp.ViewModels
{
    public class RemindersViewModel : INotifyPropertyChanged
    {
        private PlotModel _waterIntakeModel;
        private double _consumedWater;
        private double _targetWater;

        public RemindersViewModel()
        {
            _targetWater = 2000; // Set your target water intake
            _consumedWater = 500; // Set the initial consumed water amount

            CreateWaterIntakeModel();
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
            
<<<<<<< Updated upstream
            var consumedSlice = new PieSlice("Consumed", _consumedWater)
=======
            var consumedSlice = new PieSlice("Drinked", _consumedWater)
>>>>>>> Stashed changes
            {
                IsExploded = true,
                Fill = OxyColor.Parse("#0E87CC") // The color for the "Consumed" slice
            };

            var remainingSlice = new PieSlice("Remaining", _targetWater - _consumedWater)
            {
                Fill = OxyColor.Parse("#BDBDBD") // The color for the "Remaining" slice
            };

            var pieSeries = new PieSeries
            {
                Slices = new List<PieSlice> { consumedSlice, remainingSlice },
                StrokeThickness = 2.0,
<<<<<<< Updated upstream
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0,
                FontSize = 15
=======
                InsideLabelPosition = 0.6,                
                AngleSpan = 360,
                StartAngle = 0,
                FontSize = 18,                
>>>>>>> Stashed changes
            };

            _waterIntakeModel.Series.Add(pieSeries);

            _waterIntakeModel.InvalidatePlot(true); // This refreshes the plot
            OnPropertyChanged(nameof(WaterIntakeModel));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}