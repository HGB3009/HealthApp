﻿using HealthCareApp.Models;
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
using System.Windows.Input;
using System.Net.Http.Json;
using System.ComponentModel;
using static HealthCareApp.Models.SymptomCheckerModel.Autherity;
using System.Windows.Controls;
using MongoDB.Driver;

namespace HealthCareApp.ViewModels
{
    public class SymptomCheckerViewModel : BaseViewModel
    {
        public ICommand DiagnosisBtnCommand { get; set; }
        public ICommand SelectionChangedItemCommand { get; set; }
        public ICommand AddSymptomBtnCommand { get; set; }
        public ICommand ClearSymptomsBtnCommand { get; set; }

        public List<string> tempItem = new List<string>();
        public List<string> Items
        {
            get { return tempItem; }
            set
            {
                tempItem = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        public string tempSymptoms;
        public string TempSymptoms
        {
            get { return tempSymptoms; }
            set
            {
                tempSymptoms = value;
                OnPropertyChanged(nameof(TempSymptoms));
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
        public string resultDiagnosis;
        public string ResultDiagnosis
        {
            get { return resultDiagnosis; }
            set
            {
                resultDiagnosis = value;
                OnPropertyChanged(nameof(ResultDiagnosis));
            }
        }
        public string descriptionIssue;
        public string DescriptionIssue
        {
            get { return descriptionIssue; }
            set
            {
                descriptionIssue = value;
                OnPropertyChanged(nameof(DescriptionIssue));
            }
        }
        public string issueName;
        public string IssueName
        {
            get { return issueName; }
            set
            {
                issueName = value;
                OnPropertyChanged(nameof(IssueName));
            }
        }
        public string issueAcc;
        public string IssueAcc
        {
            get { return issueAcc; }
            set
            {
                issueAcc = value;
                OnPropertyChanged(nameof(IssueAcc));
            }
        }
        public string adviceTreatment;
        public string AdviceTreatment
        {
            get { return adviceTreatment; }
            set
            {
                adviceTreatment = value;
                OnPropertyChanged(nameof(AdviceTreatment));
            }
        }
        public string possibleSymptoms;
        public string PossibleSymptoms
        {
            get { return possibleSymptoms; }
            set
            {
                possibleSymptoms = value;
                OnPropertyChanged(nameof(PossibleSymptoms));
            }
        }
        bool successDiagnosis = false;
        public string Gender;
        public string BirthYear;
        private readonly IMongoCollection<UserInformation> _userinfoCollection;
        public SymptomCheckerViewModel()
        {
            LoadToken();
            LoadListSymptom();

            _userinfoCollection = GetMongoCollectionFromUserInfo();
            string username=Const.Instance.Username;
            var filter = Builders<UserInformation>.Filter.Eq(x => x.Username, username);
            var User = _userinfoCollection.Find(filter).FirstOrDefault();
            if (User != null)
            {
                string birthdayString = User.Birthday;
                if (DateTime.TryParseExact(birthdayString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime birthday))
                {
                    BirthYear=birthday.Year.ToString();
                }
                Gender = User.Gender;
            }
            DiagnosisBtnCommand = new RelayCommand<TextBox>((p) => { return (p == null || p.Text == string.Empty) ? false : true; }, (p) => {
                DiagnosisBtn_Click();
            });
            AddSymptomBtnCommand = new RelayCommand<ComboBox>((p) => { return true; }, (p) =>
            {
                if (p.SelectedItem != null)
                    UpdateSymptomsBox(p.SelectedItem.ToString());
            });
            ClearSymptomsBtnCommand = new RelayCommand<object>((p) => { return true; }, (p) => { TempSymptoms = string.Empty; });
        }
        string api_key = "w8HRx_GM_UIT_EDU_VN_AUT";
        string secret_key = "g9C3SxWc4b5Q8EwJt";
        string url = "https://healthservice.priaid.ch/";
        string token;

        Autherity autherity;
        rootSymptoms ListSymptoms = new rootSymptoms();
        issueInfo curIssueInfo;

        private async void LoadToken()
        {
            token = await getToken();
        }
        private async void LoadListSymptom()
        {
            string result = await getListSymptoms(url, token);
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
                tempItem.Add(Nametemp);
                ListSymptoms.symptoms.Add(newSymptoms);
            }
        }
        private async void DiagnosisBtn_Click()
        {
            successDiagnosis = false;
            string fewSymptoms = ConvertSymptomToID(TempSymptoms);
            string gender = Gender;
            string yearOfBirth = BirthYear;
            string result = await getDiagnosis(url, autherity.Token, gender, yearOfBirth, fewSymptoms);
            if (!successDiagnosis || result.Length < 5)
            {
                MessageBox.Show("Cannot Diagnosis. Please check again your symptoms infomation.");
                ShowBadRequest();
            }
            else
            {
                int topAccIssueID = getTopIssueID(result, 2);
                double topAccIssue = getTopIssueAcc(result, 4);
                LoadIssueInfo(topAccIssueID);
                IssueAcc = topAccIssue.ToString() + "%";
            }
        }
        private async Task<string> getDiagnosis(string url, string token, string gender, string yearOfBirth, string fewSymtomps)
        {
            using (HttpClient client = new HttpClient())
            {
                var uri = new Uri($"{url}diagnosis?token={token}&language=en-gb&symptoms=[{fewSymtomps}]&gender={gender}&year_of_birth={yearOfBirth}&format=json");
                HttpResponseMessage responseMessage = await client.GetAsync(uri);
                if (responseMessage.IsSuccessStatusCode)
                {
                    successDiagnosis = true;
                    return await responseMessage.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error:{responseMessage.StatusCode},{await responseMessage.Content.ReadAsStringAsync()}";
                }

            }
        }
        private async void LoadIssueInfo(int issue_id)
        {
            string result = await getIssueInfo(issue_id);
            curIssueInfo = JsonConvert.DeserializeObject<issueInfo>(result);
            IssueName = curIssueInfo.Name;
            DescriptionIssue = curIssueInfo.DescriptionShort;
            AdviceTreatment = curIssueInfo.TreatmentDescription;
            PossibleSymptoms = curIssueInfo.PossibleSymptoms;
        }
        private async Task<string> getIssueInfo(int issue_id)
        {
            using (HttpClient client = new HttpClient())
            {
                var uri = new Uri($"{url}issues/{issue_id}/info?token={token}&language=en-gb");
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
        private void UpdateSymptomsBox(string element)
        {
            Char.ToUpper(element[0]);
            string s = TempSymptoms + ((TempSymptoms != null && TempSymptoms != string.Empty) ? ", " : "") + element;
            TempSymptoms = s;
        }
        private string ConvertSymptomToID(string Symptoms)
        {
            string listID = "";
            string[] elementSymptoms = Symptoms.Split(", ");
            for (int i = 0; i < elementSymptoms.Length; ++i)
            {
                string temp = elementSymptoms[i].ToLower();
                for (int j = 0; j < ListSymptoms.symptoms.Count; ++j)
                {
                    if (temp == ListSymptoms.symptoms[j].Name.ToLower())
                    {
                        listID += ((i != 0) ? "," : "") + ListSymptoms.symptoms[j].ID.ToString();
                        break;
                    }
                }
            }
            return listID;
        }
        private int getTopIssueID(string issueList, int num_dot) // numdot=2 : lay ID; numdot=4: lay Acc
        {
            string result = "";
            int dot = 0;
            int i = 0;
            while (dot != num_dot)
            {
                if (issueList[i] == ':')
                    dot++;
                ++i;
            }
            while (issueList[i] != ',')
            {
                result += issueList[i];
                ++i;
            }
            return int.Parse(result);
        }
        private double getTopIssueAcc(string issueList, int num_dot) // numdot=2 : lay ID; numdot=4: lay Acc
        {
            string result = "";
            int dot = 0;
            int i = 0;
            while (dot != num_dot)
            {
                if (issueList[i] == ':')
                    dot++;
                ++i;
            }
            while (issueList[i] != ',')
            {
                result += issueList[i];
                ++i;
            }
            return double.Parse(result);
        }
        private void ShowBadRequest()
        {
            IssueAcc = "";
            IssueName = "";
            DescriptionIssue = "";
            AdviceTreatment = "";
            PossibleSymptoms = "";
        }
        private IMongoCollection<UserInformation> GetMongoCollectionFromUserInfo()
        {
            // Set your MongoDB connection string and database name
            string connectionString = "mongodb+srv://HGB3009:HGB30092004@bao-database.xwrghva.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "HealthcareManagementDatabase"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<UserInformation>("UserInformation");
        }
    }
}