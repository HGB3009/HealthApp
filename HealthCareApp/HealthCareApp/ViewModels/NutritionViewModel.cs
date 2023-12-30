using HealthCareApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthCareApp.ViewModels
{

    public class NutritionViewModel : BaseViewModel
    {
        public ICommand AddMealCommand { get; set; }
        public ICommand CalculateCommand { get; set; }

        public NutritionViewModel()
        {
            AddMealCommand = new RelayCommand<NutritionView>((p) => { return true; }, (p) => AddMeal(p));
            CalculateCommand = new RelayCommand<NutritionView>((p) => { return true; }, (p) => Calculate(p));
        }
        public void AddMeal(NutritionView parameter)
        {
            AddMealView addMealView = new AddMealView();
            parameter.Opacity = 0.5;
            addMealView.ShowDialog();
            parameter.Opacity = 1;
        }
        public void Calculate(NutritionView parameter)
        {
            CalculateCaloriesView calculateCaloriesView = new CalculateCaloriesView();
            parameter.Opacity = 0.5;
            calculateCaloriesView.ShowDialog();
            parameter.Opacity = 1;
        }
    }
}
