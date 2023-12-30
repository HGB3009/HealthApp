using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static HealthCareApp.Models.ExerciseModel;

namespace HealthCareApp.ViewModels
{
    internal class AddExerciseViewModel : BaseViewModel
    {
        public ICommand ExitBtnCommand { get; set; }
        public AddExerciseViewModel()
        {
            ExitBtnCommand=new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
            getExercise();
        }
        string api_key = "2qhorCkNJtS7gPC5veMb0w==vuH8jN3gfZBHCzzh";
        string url = "https://api.api-ninjas.com/v1/exercises?";
        public rootExercises ListExercise;
        private async void getExercise()
        {
            string result = await requestExercise("press", "strength", "biceps", "beginner");
        }
        private async Task<string> requestExercise(string name, string type, string muscle, string difficulty)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Api-Key", api_key);

                var uri = new Uri($"{url}muscle={muscle}&type={type}&difficulty={difficulty}");
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error:{response.StatusCode},{await response.Content.ReadAsStringAsync()}";
                }
            }
        }
        private rootExercises ConvertData(string data)
        {
            rootExercises result = new rootExercises();
            int i = 2;
            int pair = 0;
            string element = "";
            while (data[i] != ']')
            {
                if (data[i] == '}')
                    if (pair < 2) { element += data[i]; ++i; }
                    else
                    {

                    }
            }
            return result;
        }
    }
}
