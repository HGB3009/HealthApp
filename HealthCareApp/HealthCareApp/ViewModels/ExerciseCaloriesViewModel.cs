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

namespace HealthCareApp.ViewModels
{
    public class ExerciseCaloriesViewModel:BaseViewModel
    {

        public ExerciseCaloriesViewModel()
        {

        }
        private string Nutrient_API_ID = "1c2aa54e";
        private string Nutrient_API_Key = "146bf4035a385f1d5632b717d12f3a4a";
        private string endPoint = "exercise";
        private string Nutrient_url = $"https://trackapi.nutritionix.com/v2/natural/";
        rootExerciseInfo ListExerciseInfo;
        private async void calculate(string querry)
        {
            string result = await getExerciseInfo(querry);
            ListExerciseInfo = JsonConvert.DeserializeObject<rootExerciseInfo>(result);
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

    }
}
