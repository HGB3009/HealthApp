using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net.Http.Json;
using HealthCareApp.Models;
using System.IO;
using System.Windows.Input;
using HealthCareApp.Views;

namespace HealthCareApp.ViewModels
{
    public class NutrientsViewModel : BaseViewModel
    {
        public ICommand AddMealCommand { get; set; }
        public ICommand CalculateCommand { get; set; }

        public NutrientsViewModel()
        {
            AddMealCommand=new RelayCommand<NutrientsView>((p) => { return true; },(p)=> {
                AddMealView addMealView = new AddMealView();
                addMealView.ShowDialog();
            });
            CalculateCommand = new RelayCommand<NutrientsView>((p) => { return true; }, (p) =>
            {
                CalculateCaloriesView calculateCaloriesView = new CalculateCaloriesView();
                calculateCaloriesView.ShowDialog();
            });
        }
    }
}
