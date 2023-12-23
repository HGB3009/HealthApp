using HealthCareApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HealthCareApp.ViewModels;
using System.Windows.Input;
namespace HealthCareApp.ViewModels
{
    public class MainViewModel:BaseViewModel
    {
        public bool isLoaded = false;
        public ICommand LoadedWindownCommand {  get; set; }
        public MainViewModel()
        {
            SignInView signInView = new SignInView();
            SymtompCheckerView symtompCheckerView = new SymtompCheckerView();
         
            if (!isLoaded)
            {
                isLoaded = true;
                //signInView.ShowDialog();
                symtompCheckerView.ShowDialog();
            }
        }
    }
}
