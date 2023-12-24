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
    internal class SignInViewModel:BaseViewModel
    {
        public ICommand OpenSignUpWindowCommand { get; set; }
        public ICommand OpenChangePasswordCommand { get; set; }
        public SignInViewModel() 
        {
            OpenSignUpWindowCommand = new RelayCommand<SignInView>((parameter) => true, (parameter) => OpenSignUpWindow(parameter));
            OpenChangePasswordCommand = new RelayCommand<SignInView>((parameter) => true, (parameter) => OpenForgotPasswordWindow(parameter));
        }
        public void OpenForgotPasswordWindow(Window loginWindow)
        {
            ForgotPasswordView forgotPasswordWindow = new ForgotPasswordView();

            //forgotPasswordWindow.Username.Text = null;
            loginWindow.Opacity = 0.5;
            loginWindow.WindowStyle = WindowStyle.None;
            forgotPasswordWindow.ShowDialog();
            loginWindow.WindowStyle = WindowStyle.SingleBorderWindow;
            loginWindow.Opacity = 1;
            loginWindow.Show();
        }
        public void OpenSignUpWindow(Window loginWindow)
        {
            SignupView signupWindow = new SignupView();

            //forgotPasswordWindow.Username.Text = null;
            loginWindow.Opacity = 0.5;
            loginWindow.WindowStyle = WindowStyle.None;
            signupWindow.ShowDialog();
            loginWindow.WindowStyle = WindowStyle.SingleBorderWindow;
            loginWindow.Opacity = 1;
            loginWindow.Show();
            
        }
    }
}
