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
using System.Windows.Controls;
using Amazon.Util.Internal;

namespace HealthCareApp.ViewModels
{
    public class CalculateCaloriesViewModel : BaseViewModel
    {
        public ICommand IngredientTextChangedCommand { get; set; }
        public ICommand CalculateBtnCommand { get; set; }
        public ICommand FoodNameChangedCommand { get; set; }
        public ICommand QuantityChangedCommand { get; set; }
        public ICommand UnitChangedCommand { get; set; }
        public ICommand CancelCalculateCommand { get; set; }

        public string foodNameTextBox;
        public string FoodNameTextBox
        {
            get { return foodNameTextBox; }
            set
            {
                foodNameTextBox = value;
                OnPropertyChanged(nameof(FoodNameTextBox));
            }
        }
        public string quantityTextBox;
        public string QuantityTextBox
        {
            get { return quantityTextBox; }
            set
            {
                quantityTextBox = value;
                OnPropertyChanged(nameof(QuantityTextBox));
            }
        }

        public string unitTextBox;
        public string UnitTextBox
        {
            get { return unitTextBox; }
            set
            {
                unitTextBox = value;
                OnPropertyChanged(nameof(UnitTextBox));
            }
        }
        public string ingredientTextBox;
        public string IngredientTextBox
        {
            get { return ingredientTextBox; }
            set
            {
                ingredientTextBox = value;
                OnPropertyChanged(nameof(IngredientTextBox));
            }
        }
        public string totalFat;
        public string TotalFat { get { return totalFat; } set { totalFat = value; OnPropertyChanged(nameof(TotalFat)); } }

        public string sumCalories;
        public string SumCalories { get { return sumCalories; } set { sumCalories = value; OnPropertyChanged(nameof(SumCalories)); } }
        public string saturatedFat;
        public string SaturatedFat { get { return saturatedFat; } set { saturatedFat = value; OnPropertyChanged(nameof(SaturatedFat)); } }
        public string totalCholesterol;
        public string TotalCholesterol { get { return totalCholesterol; } set { totalCholesterol = value; OnPropertyChanged(nameof(TotalCholesterol)); } }
        public string totalSodium;
        public string TotalSodium { get { return totalSodium; } set { totalSodium = value; OnPropertyChanged(nameof(TotalSodium)); } }
        public string totalCacbohydrates;
        public string TotalCacbohydrates { get { return totalCacbohydrates; } set { totalCacbohydrates = value; OnPropertyChanged(nameof(TotalCacbohydrates)); } }
        public string totalSugars;
        public string TotalSugars { get { return totalSugars; } set { totalSugars = value; OnPropertyChanged(nameof(TotalSugars)); } }
        public string totalProtein;
        public string TotalProtein { get { return totalProtein; } set { totalProtein = value; OnPropertyChanged(nameof(TotalProtein)); } }

        public string totalPotassium;
        public string TotalPotassium { get { return totalPotassium; } set { totalPotassium = value; OnPropertyChanged(nameof(TotalPotassium)); } }

        public string totalDietaryFiber;
        public string TotalDietaryFiber { get { return totalDietaryFiber; } set { totalDietaryFiber = value; OnPropertyChanged(nameof(TotalDietaryFiber)); } }
        public string totalFatTrans;
        public string TotalFatTrans { get { return totalFatTrans; } set { totalFatTrans = value; OnPropertyChanged(nameof (TotalFatTrans)); } }
        public string totalVitaminD;
        public string TotalVitaminD { get { return totalVitaminD; } set { totalVitaminD = value; OnPropertyChanged(nameof(TotalVitaminD)); } }
        public string totalCalcium;
        public string TotalCalcium { get { return totalCalcium; } set { totalCalcium = value; OnPropertyChanged(nameof(TotalCalcium)); } }
        public string totalIron;
        public string TotalIron { get { return totalIron; } set { totalIron = value; OnPropertyChanged(nameof(TotalIron)); } }
        public string totalPolyunsaturated;
        public string TotalPolyunsaturated { get { return totalPolyunsaturated; } set { totalPolyunsaturated = value; OnPropertyChanged(nameof(TotalPolyunsaturated)); } }
        public string totalMonounsaturated;
        public string TotalMonounsaturated { get { return totalMonounsaturated; } set { totalMonounsaturated = value; OnPropertyChanged(nameof(TotalMonounsaturated)); } }
        public string totalPhosphorus;
        public string TotalPhosphorus { get { return totalPhosphorus; } set { totalPhosphorus = value; OnPropertyChanged(nameof(TotalPhosphorus)); } }
        public string hintIngredient;
        public string HintIngredient { get { return hintIngredient; } set { hintIngredient = value; OnPropertyChanged(nameof (HintIngredient)); } }
        public CalculateCaloriesViewModel()
        {
            HintIngredient = "1 cup of rice" + '\n' + "10 oz of chickbeans";
            IngredientTextChangedCommand = new RelayCommand<TextBlock>((p) => { return true; }, (p) =>
            {
                FoodNameTextBox = "";
                QuantityTextBox = "";
                UnitTextBox = "";
            });
            CalculateBtnCommand = new RelayCommand<TextBox>((p) => { return (((IngredientTextBox == string.Empty || IngredientTextBox == null) && (p == null||p.Text==string.Empty)) &&
                                                                                (FoodNameTextBox == string.Empty || QuantityTextBox == string.Empty ||
                                                                                FoodNameTextBox == null || QuantityTextBox == null)) ? false : true; }, (p) =>
            {
                if (IngredientTextBox == null || IngredientTextBox == string.Empty)
                {
                    string name = FoodNameTextBox;
                    string quantity = QuantityTextBox;
                    string unit = (UnitTextBox != null) ? UnitTextBox : "";
                    string querry = ConvertFoodIngredient(name, quantity, unit);
                    CalculateCaloriesNutrient(querry);
                    List<string>querryEdamam = new List<string>();
                    querryEdamam.Add(querry);
                    CalculateCaloriesEdamam(querryEdamam);
                }
                else
                {
                    string convertNutrient = ConvertIngredientTextNutrient(IngredientTextBox);
                    CalculateCaloriesNutrient(convertNutrient);
                    List<string> ConvertEdamam=ConvertIngredientTextEdamam(IngredientTextBox);
                    CalculateCaloriesEdamam(ConvertEdamam);
                }
            });
            FoodNameChangedCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IngredientTextBox = "";
            });
            QuantityChangedCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IngredientTextBox = "";
            });
            UnitChangedCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IngredientTextBox = "";
            });
            CancelCalculateCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                p.Close();
            });
        }

        private string Nutrient_API_ID = "1c2aa54e";
        private string Nutrient_API_Key = "146bf4035a385f1d5632b717d12f3a4a";
        private string endPoint;
        private string Nutrient_url = $"https://trackapi.nutritionix.com/v2/natural/";

        private string Edamam_appId = "9541ae7b";
        private string Edamam_apiKey = "80938161b72eb2819453ebff5678e7cb";
        private string Edamam_apiUrl = "https://api.edamam.com/api/nutrition-data";

        bool sucessGetCalculate;
        bool successCalculateEdamam;
        NutrientsModel.root NutrientSource;
        private string ConvertFoodIngredient(string name, string quantity, string unit)
        {
            string result = "";
            result = quantity + ' ' + ((unit != "") ? unit : "") + " of " + name;
            return result;
        }
        private async void CalculateCaloriesNutrient(string ingredients)
        {
            sucessGetCalculate = false;
            string query = ingredients;
            endPoint = "nutrients";
            string result = await GetNutritionInfo(query);
            if (sucessGetCalculate)
            {
                NutrientSource = JsonConvert.DeserializeObject<NutrientsModel.root>(result);
                NutrientsModel.root temp = NutrientSource;
                ShowCalculateResult(temp);
            }
            else
            {
                MessageBox.Show(result);
            }
        }

        private async Task<string> GetNutritionInfo(string query)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-app-id", Nutrient_API_ID);
                client.DefaultRequestHeaders.Add("x-app-key", Nutrient_API_Key);

                var parameters = new
                {
                    query = query,
                };
                var uri = new Uri($"{Nutrient_url}{endPoint}");
                HttpResponseMessage response = await client.PostAsJsonAsync(uri, parameters);
                if (response.IsSuccessStatusCode)
                {
                    sucessGetCalculate = true;
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error: {response.StatusCode},{await response.Content.ReadAsStringAsync()}";
                }
            }
        }
        private async void CalculateCaloriesEdamam(List<string> Items)
        {
            double Calcium = 0;
            double VitaminD = 0;
            double Iron = 0;
            double FatTrans = 0;
            double Polyunsaturated = 0;
            double Monounsaturated = 0;
            double Phosphorus = 0;
            foreach (string Item in Items)
            {
                successCalculateEdamam = false;
                string result=await GetInfoEdamamAPI(Item);
                if (successCalculateEdamam)
                {
                    Calcium += findNutrient(result,"Calcium");
                    VitaminD += findNutrient(result, "Vitamin D");
                    Iron += findNutrient(result, "Iron");
                    FatTrans += findNutrient(result, "total trans");
                    Polyunsaturated += findNutrient(result, "polyunsaturated");
                    Monounsaturated += findNutrient(result, "monounsaturated");
                    Phosphorus += findNutrient(result, "Phosphorus");
                }
            }
            Calcium = Math.Round(Calcium, 2);
            VitaminD = Math.Round(VitaminD, 2);
            Iron = Math.Round(Iron, 2);
            FatTrans = Math.Round(FatTrans, 2);
            Polyunsaturated = Math.Round(Polyunsaturated, 2);
            Monounsaturated = Math.Round(Monounsaturated, 2);
            Phosphorus = Math.Round(Phosphorus, 2);

            TotalFatTrans = FatTrans.ToString() + 'g';
            TotalIron = Iron.ToString() + "mg";
            TotalVitaminD = VitaminD.ToString() + "mcg";
            TotalCalcium = Calcium.ToString() + "mg";
            TotalPolyunsaturated = Polyunsaturated.ToString() + 'g';
            TotalMonounsaturated = Monounsaturated.ToString() + 'g';
            TotalPhosphorus = Phosphorus.ToString() + "mg";
        }
        private async Task<string> GetInfoEdamamAPI(string foodItem)
        {
            using (HttpClient client = new HttpClient())
            {
                var parameters = new
                {
                    app_id = Edamam_appId,
                    app_key = Edamam_apiKey,
                    ingr = foodItem
                };

                var uri = new Uri($"{Edamam_apiUrl}?app_id={Edamam_appId}&app_key={Edamam_apiKey}&ingr={foodItem}");
                HttpResponseMessage response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    successCalculateEdamam = true;
                    return await response.Content.ReadAsStringAsync();
                }
                else
                    return $"Error:{response.StatusCode},{await response.Content.ReadAsStringAsync()}";
            }
        }
        private double findNutrient(string text,string Nutrient)
        {
            string nutri = "";
            int index = text.IndexOf(Nutrient);
            if (index == -1)
                return 0;
            else
            {
                int j = index;
                while (text[j] != ':' && j < text.Length)
                {
                    ++j;
                }
                ++j;
                while (text[j] != ',')
                {
                    nutri += text[j];
                    ++j;
                }
                return double.Parse(nutri);
            }
        }
        private string ConvertIngredientTextNutrient(string text)
        {
            string result = "";
            string[] Listtext = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None); 
            for(int i = 0;i<Listtext.Length; i++)
            {
                result += ((i != 0) ? ". " : "") + Listtext[i];
            }
            return result;
        }
        private List<string> ConvertIngredientTextEdamam(string text)
        {
            string[] Listtext = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            List<string> result = new List<string>();
            for(int i=0;i<Listtext.Length;++i)
                result.Add(Listtext[i]);
            return result;
        }
        private void ShowCalculateResult(NutrientsModel.root result)
        {
            SumCalories = CalculateSumCalories(result).ToString()+" kcal";
            TotalFat = CalculateTotalFat(result).ToString() + 'g';
            SaturatedFat = CalculateTotalSaturatedFat(result).ToString() + 'g';
            TotalCholesterol = CalculateTotalCholesterol(result).ToString() + "mg";
            TotalSodium = CalculateTotalSodium(result).ToString() + "mg";
            TotalCacbohydrates = CalculateTotalCacbohydrates(result).ToString() + 'g';
            TotalSugars = CalculateTotalSugar(result).ToString() + 'g';
            TotalProtein = CalculateTotalProtein(result).ToString() + 'g';
            TotalPotassium = CalculateTotalPotassium(result).ToString() + "mg";
            TotalDietaryFiber = CalculateTotalDietaryFiber(result).ToString() + 'g';
        }
        private double CalculateSumCalories(NutrientsModel.root result)
        {
            double sum = 0;
            for (int i = 0; i < result.foods.Count; i++)
            {
                sum += result.foods[i].nf_calories;
            }
            return sum;
        }
        private double CalculateTotalFat(NutrientsModel.root result)
        {
            double sum = 0;
            for (int i = 0; i < result.foods.Count; i++)
            {
                sum += result.foods[i].nf_total_fat;
            }
            return sum;
        }
        private double CalculateTotalSaturatedFat(NutrientsModel.root result)
        {
            double sum = 0;
            for (int i = 0; i < result.foods.Count; i++)
            {
                sum += result.foods[i].nf_saturated_fat;
            }
            return sum;
        }
        private double CalculateTotalCholesterol(NutrientsModel.root result)
        {
            double sum = 0;
            for (int i = 0; i < result.foods.Count; i++)
            {
                sum += result.foods[i].nf_cholesterol;
            }
            return sum;
        }
        private double CalculateTotalSodium(NutrientsModel.root result)
        {
            double sum = 0;
            for (int i = 0; i < result.foods.Count; i++)
            {
                sum += result.foods[i].nf_sodium;
            }
            return sum;
        }
        private double CalculateTotalCacbohydrates(NutrientsModel.root result)
        {
            double sum = 0;
            for (int i = 0; i < result.foods.Count; i++)
            {
                sum += result.foods[i].nf_total_carbohydrate;
            }
            return sum;
        }
        private double CalculateTotalSugar(NutrientsModel.root result)
        {
            double sum = 0;
            for (int i = 0; i < result.foods.Count; i++)
            {
                sum += result.foods[i].nf_sugars;
            }
            return sum;
        }
        private double CalculateTotalProtein(NutrientsModel.root result)
        {
            double sum = 0;
            for (int i = 0; i < result.foods.Count; i++)
            {
                sum += result.foods[i].nf_protein;
            }
            return sum;
        }
        private double CalculateTotalPotassium(NutrientsModel.root result)
        {
            double sum = 0;
            for (int i = 0; i < result.foods.Count; i++)
            {
                sum += result.foods[i].nf_potassium;
            }
            return sum;
        }
        private double CalculateTotalDietaryFiber(NutrientsModel.root result)
        {
            double sum = 0;
            for (int i = 0; i < result.foods.Count; i++)
            {
                sum += result.foods[i].nf_dietary_fiber;
            }
            return sum;
        }
    }
}
