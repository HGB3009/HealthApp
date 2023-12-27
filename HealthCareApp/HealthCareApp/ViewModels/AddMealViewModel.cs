using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HealthCareApp.ViewModels
{
    internal class AddMealViewModel:BaseViewModel
    {
        public ICommand CancelAddMealCommand { get; set; }
        public AddMealViewModel()
        {
            CancelAddMealCommand=new RelayCommand<Window>((p) => { return true; },(p)=> { p.Close(); });
        }
    }
}
