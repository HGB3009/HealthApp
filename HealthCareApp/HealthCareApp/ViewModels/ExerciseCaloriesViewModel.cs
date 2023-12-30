using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static HealthCareApp.Models.ExerciseModel;
using Newtonsoft.Json;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace HealthCareApp.ViewModels
{
    public class ExerciseCaloriesViewModel:BaseViewModel
    {
        public ICommand CancelBtnCommand { get; set; }
        public ICommand CalculateBtnCommand { get; set; }
        public ExerciseCaloriesViewModel()
        {
            HintExercise = "swam for 1 hour" + '\n' + "30 push-ups";
            CancelBtnCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
            CalculateBtnCommand=new RelayCommand<TextBox>((p) => { return (p == null || p.Text == string.Empty) ? false : true; }, (p) =>
            {
                ShowNullResult();
                List<string> exercises=ConvertActivities(p.Text);
                calculate(exercises);
            });
        }
        public string listExerciseAct;
        public string ListExerciseAct { get { return listExerciseAct; } set { listExerciseAct = value; OnPropertyChanged(nameof(ListExerciseAct)); } }

        public string hintExercise;
        public string HintExercise { get { return hintExercise; } set { hintExercise = value; OnPropertyChanged(nameof(HintExercise)); } }

        public string activitiesList;
        public string ActivitiesList { get { return activitiesList; } set { activitiesList = value; OnPropertyChanged(nameof(ActivitiesList)); } }

        public string caloriesList;
        public string CaloriesList { get { return caloriesList; } set { caloriesList = value; OnPropertyChanged(nameof(CaloriesList)); } }

        public string totalCalories;
        public string TotalCalories { get { return totalCalories; } set { totalCalories = value; OnPropertyChanged(nameof(TotalCalories)); } }  

        private string Nutrient_API_ID = "1c2aa54e";
        private string Nutrient_API_Key = "146bf4035a385f1d5632b717d12f3a4a";
        private string endPoint = "exercise";
        private string Nutrient_url = $"https://trackapi.nutritionix.com/v2/natural/";

        rootExerciseInfo ListExerciseInfo;


        string tempAct;
        string tempActKcal;
        double tempTotalKcal;
        private async void calculate(List<string> querry)
        {
            tempAct = "";
            tempActKcal = "";
            tempTotalKcal = 0;
            int i = 0;
            foreach(string act in querry)
            {
                string result = await getExerciseInfo(act);
                ListExerciseInfo = JsonConvert.DeserializeObject<rootExerciseInfo>(result);
                AddCalculate(ListExerciseInfo,i);
                ++i;
            }
            ShowCalculate(tempAct,tempActKcal);
        }
        private async Task<string> getExerciseInfo(string querry)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-app-id", Nutrient_API_ID);
                client.DefaultRequestHeaders.Add("x-app-key", Nutrient_API_Key);

                var parameters = new
                {
                    query = querry,
                };
                var uri = new Uri($"{Nutrient_url}{endPoint}");
                HttpResponseMessage response = await client.PostAsJsonAsync(uri, parameters);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error: {response.StatusCode},{await response.Content.ReadAsStringAsync()}";
                }
            }
        }
        private List<string> ConvertActivities(string act)
        {
            string[] list = act.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            return list.ToList();
        }
        private void AddCalculate(rootExerciseInfo ListExerciseInfo,int i)
        {
            tempAct += ((i != 0) ? '\n' : "") + ListExerciseInfo.exercises[0].name;
            tempActKcal+= ((i != 0) ? '\n' : "") + ListExerciseInfo.exercises[0].nf_calories.ToString();
            tempTotalKcal += ListExerciseInfo.exercises[0].nf_calories;
        }
        private void ShowCalculate(string tempAct,string tempKcal)
        {
            ActivitiesList = tempAct;
            CaloriesList = tempActKcal;
            tempTotalKcal = Math.Round(tempTotalKcal, 2);
            TotalCalories = tempTotalKcal.ToString();
        }
        private void ShowNullResult()
        {
            ActivitiesList = "";
            CaloriesList = "";
            TotalCalories = "";
        }
    }
}
