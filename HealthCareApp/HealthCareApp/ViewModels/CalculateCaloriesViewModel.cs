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
using System.Windows.Input;
using HealthCareApp.Views;

namespace HealthCareApp.ViewModels
{
    public class CalculateCaloriesViewModel:BaseViewModel
    {
        public string tempIngredient;
        public string IngredientTextBox {
            get { return tempIngredient; }
            set
            {
                tempIngredient = value;
                OnPropertyChanged(nameof(IngredientTextBox));
            }
        }
        public ICommand CalculateCommand { get; set; }
        public CalculateCaloriesViewModel() 
        {
            CalculateCommand=new RelayCommand<CalculateCaloriesView>((p) => { return true; },(p)=> { btnSearchNutrients_Click();});
        }
        string API_ID = "1c2aa54e";
        string API_Key = "146bf4035a385f1d5632b717d12f3a4a";
        string endPoint;
        string url = $"https://trackapi.nutritionix.com/v2/natural/";
        NutrientsModel.root NutrientSource;
        private async void btnSearchNutrients_Click()
        {
            string query = IngredientTextBox;
            query=ConvertStringIngredient(query);
            MessageBox.Show(query);
            endPoint = "nutrients";
            string result = await GetNutritionInfo(query);
            if (result[0] != 'E')
            {
                NutrientSource = JsonConvert.DeserializeObject<NutrientsModel.root>(result);
                MessageBox.Show("Success get Nutrient info.");
                File.WriteAllText("NutrientsInfo.txt", result);
            }
            else
                MessageBox.Show(result);
            //MessageBox.Show(NutrientSource.foods[0].food_name);
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
        private string ConvertStringIngredient(string tempquerry)
        {
            string []elementsQuerry = tempquerry.Split(',');
            string querry = "";
            for(int i = 0; i < elementsQuerry.Length; ++i)
            {
                string[] partElement = elementsQuerry[i].Split(" ");
                string realElement = "";
                for(int j=0;j< partElement.Length; ++j)
                {
                    if ((partElement[0] !=" " &&j==0)||(partElement[partElement.Length-1]==" "&&j== partElement.Length - 1))
                        continue;
                    realElement += partElement[j];
                    if(j!=partElement.Length-1)
                        realElement += " ";
                }
                querry += realElement;
                if(i!=elementsQuerry.Length-1) 
                    querry += ", ";
            }
            MessageBox.Show(querry);
            return querry;
        }
    }
}
