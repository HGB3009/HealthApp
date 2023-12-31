using HealthCareApp.Views;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthCareApp.ViewModels
{
    public class FitnessViewModel : BaseViewModel
    {
        public ICommand CalculateCaloriesCommand { get; set; }
        public ICommand TrainingBtnCommand { get; set; }
        public ICommand AddExerciseCommand { get; set; }

        public FitnessViewModel() 
        {
            CalculateCaloriesCommand=new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ExerciseCaloriesView window= new ExerciseCaloriesView();
                window.ShowDialog();
            });
            TrainingBtnCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddExerciseView window = new AddExerciseView();
                window.ShowDialog();
            });
            AddExerciseCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddLessonExerciseView window = new AddLessonExerciseView();
                window.ShowDialog();
            });
        }

    }
}
