using HealthCareApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;

namespace HealthCareApp.ViewModels
{
    public class CalculateCaloriesViewModel
    {
        public CalculateCaloriesViewModel() 
        {
        
        }
        string API_ID = "1c2aa54e";
        string API_Key = "146bf4035a385f1d5632b717d12f3a4a";
        string endPoint;
        string url = $"https://trackapi.nutritionix.com/v2/natural/";
        NutrientsModel.root NutrientSource;
        private async void btnSearchNutrients_Click(object sender, RoutedEventArgs e)
        {
            string query = "";
            endPoint = "nutrients";
            string result = await GetNutritionInfo(query);
            NutrientSource = JsonConvert.DeserializeObject<NutrientsModel.root>(result);
            //MessageBox.Show(NutrientSource.foods[0].food_name);
            MessageBox.Show("Success get Nutrient info.");
            File.WriteAllText("NutrientsInfo.txt", result);
        }

        private async Task<string> GetNutritionInfo(string query)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-app-id", API_ID);
                client.DefaultRequestHeaders.Add("x-app-key", API_Key);

                var parameters = new
                {
                    query = query,
                };
                var uri = new Uri($"{url}{endPoint}");
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
