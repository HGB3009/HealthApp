using HealthCareApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static HealthCareApp.Models.SymptomCheckerModel;
using Newtonsoft.Json;
using System.IO;

namespace HealthCareApp.ViewModels
{
    internal class SymptomCheckerViewModel:BaseViewModel
    {
        public List<string> temp = new List<string>(); 
        public List<string> Items
        {
            get { return temp; }
            set
            {
                temp = value;
                OnPropertyChanged("Items");
            }
        }
        public string curSymptoms;
        public string dataOfSymptomBox
        {
            get { return curSymptoms; }
            set
            {
                curSymptoms = value;
                OnPropertyChanged("dataOfSymptomBox");
            }
        }
        public string selectedItem="";
        public void SelectedItemChanged()
        {
            curSymptoms += '\n' + selectedItem;
           
        }
        bool successAccess = false;
        public SymptomCheckerViewModel()
        {
            LoadToken();
            LoadListSymptom();
        }
        string api_key = "Ff24Y_GMAIL_COM_AUT";
        string secret_key = "Mi6b9GEz38Lfm7ZKd";
        string url = "https://healthservice.priaid.ch/";
        string token;

        Autherity autherity;
        rootSymptoms ListSymptoms=new rootSymptoms();
        
        private async void LoadToken()
        {
            token = await getToken();
        }
        private async void LoadListSymptom()
        {
            string result = await getListSymptoms(url, token);
            File.WriteAllText("DataSymptoms.txt", result);
            ConverToObject(result);
        }
        private async Task<string> getToken()
        {
            string uri = "https://authservice.priaid.ch/login";
            byte[] secretBytes = Encoding.UTF8.GetBytes(secret_key);
            string computedHashString = "";
            using (HMACMD5 hmac = new HMACMD5(secretBytes))
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(uri);
                byte[] computedHash = hmac.ComputeHash(dataBytes);
                computedHashString = Convert.ToBase64String(computedHash);
            }

            using (WebClient client = new WebClient())
            {
                client.Headers["Authorization"] = string.Concat("Bearer ", api_key, ":", computedHashString);
                try
                {
                    string responseArray = client.UploadString(uri, "POST", "");
                    // Deserialize token string
                    autherity = JsonConvert.DeserializeObject<Autherity>(responseArray);
                    return autherity.Token;
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }
        private async Task<string> getListSymptoms(string url, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                var urlFormat = new Uri($"{url}symptoms?token={autherity.Token}&language=en-gb");
                HttpResponseMessage responseMessage = await client.GetAsync(urlFormat);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return await responseMessage.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error:{responseMessage.StatusCode},{await responseMessage.Content.ReadAsStringAsync()}";
                }
            }
        }
        private void ConverToObject(string data)
        {
            string[] List = data.Split('"');
            for (int i = 2; i < List.Length; i += 6)
            {
                string IDtemp = List[i];
                StringBuilder stringBuilder = new StringBuilder(IDtemp);
                stringBuilder.Remove(0, 1);
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
                IDtemp = stringBuilder.ToString();
                string Nametemp = List[i + 3];
                symptom newSymptoms = new symptom();
                newSymptoms.Name = Nametemp;
                newSymptoms.ID = IDtemp;
                temp.Add(Nametemp);
                ListSymptoms.symptoms.Add(newSymptoms);
            }
        }
        public void ComboBoxSymptoms_SelectionChanged()
        {
            
        }

        //private async void btnDiagnosis_Click(object sender, RoutedEventArgs e)
        //{
        //    string fewSymptoms = SymptomsBox.Text;
        //    MessageBox.Show(fewSymptoms);
        //    string gender = Gender.Text;
        //    string yearOfBirth = YearOfBirth.Text;
        //    string token = await getToken();
        //    string result = await getDiagnosis(url, autherity.Token, gender, yearOfBirth, fewSymptoms);
        //    File.WriteAllText("Diagnosis.txt", result);
        //}
        //private async Task<string> getDiagnosis(string url, string token, string gender, string yearOfBirth, string fewSymtomps)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        var uri = new Uri($"{url}diagnosis?token={token}&language=en-gb&symptoms=[{fewSymtomps}]&gender={gender}&year_of_birth={yearOfBirth}");
        //        HttpResponseMessage responseMessage = await client.GetAsync(uri);
        //        if (responseMessage.IsSuccessStatusCode)
        //        {
        //            return await responseMessage.Content.ReadAsStringAsync();
        //        }
        //        else
        //            return $"Error:{responseMessage.StatusCode},{await responseMessage.Content.ReadAsStringAsync()}";
        //    }
        //}

    }
}
