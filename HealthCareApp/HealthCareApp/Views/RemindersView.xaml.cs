using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HealthCareApp.ViewModels;

namespace HealthCareApp.Views
{
    /// <summary>
    /// Interaction logic for RemindersView.xaml
    /// </summary>
    public partial class RemindersView : UserControl
    {
        public RemindersView()
        {
            InitializeComponent();
            DataContext = new RemindersViewModel();
        }
    }
}
