using HealthCareApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareApp.Models
{
    public class Const : BaseViewModel
    {
        private static Const _instance;

        public string Username { get; private set; }

        public static Const Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Const();
                }
                return _instance;
            }
        }

        public void SetUser(string username)
        {
            Username = username;
        }
    }
}
